using System.Net.Mail;
using FluentResults;
using Mail.Application.Metrics;
using Mail.Application.Services.Interfaces;
using Mail.Domain.Interfaces;
using Mail.Domain.Models;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Retry;
using RabbitMqContracts.Mail.Requests;
using Serilog;

namespace Mail.Application.Services;

public class MailNotificationService(IMailNotificationRepository mailNotificationRepository, IMailNotificationSender sender, IConfiguration configuration) : IMailNotificationService
{
    public async Task<Result> SendMailAsync(SendMailNotificationRequest message, CancellationToken cancellationToken)
    {
        var mailModel = new MailNotificationModel(message.Id, DateTime.UtcNow);
        await mailNotificationRepository.AddAsync(mailModel, cancellationToken);
        
        Log.Information("Отправляем уведомление с id: {@Guid}", mailModel.Id);
        try
        {
            var attempts = configuration.GetValue<int?>("Polly:DefaultAttempts") ?? 3;
            var waitTimeInSeconds = configuration.GetValue<int?>("Polly:DefaultWaitTimeInSeconds") ?? 5;
            await GetRetryPolicy(attempts, waitTimeInSeconds, mailModel)
                .ExecuteAsync(() => sender.SendAsync(message.SenderId, message.RecipientId,
                    new MailAddress(message.RecipientMail), message.Message, message.Attachments));
            
            mailModel.SetStatus(NotificationStatus.Send);
        }
        catch (Exception ex)
        {
            Log.Warning(ex.Message);
            mailModel.SetStatus(NotificationStatus.Fail);
            await mailNotificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Fail(ex.Message);
        }

        await mailNotificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        MailMetrics.SendNotificationCounter.Add(1, new KeyValuePair<string, object?>("mail.send.success", true));
        Log.Information("Успешно отправили уведомление с id: {@Guid}", mailModel.Id);
        return Result.Ok();
    }
    
    private AsyncRetryPolicy GetRetryPolicy(int attempts, int waitTimeInSeconds, MailNotificationModel mailModel)
    {
        return Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                attempts,
                _ => TimeSpan.FromSeconds(waitTimeInSeconds),
                async (exception, _, retryCount, _) =>
                {
                    Log.Warning("Retry {@0RetryCount} due to {@ExceptionMessage}", retryCount, exception.Message);
                    mailModel.SetStatus(NotificationStatus.Retry);
                    await mailNotificationRepository.UnitOfWork.SaveChangesAsync();
                    MailMetrics.SendNotificationCounter.Add(1, new KeyValuePair<string, object?>("mail.send.success", false));
                });
    }
}