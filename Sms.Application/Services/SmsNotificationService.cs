using FluentResults;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Retry;
using RabbitMqContracts.Sms.Requests;
using Serilog;
using Sms.Application.Metrics;
using Sms.Application.Services.Interfaces;
using Sms.Domain.Interfaces;
using Sms.Domain.Models;

namespace Sms.Application.Services;

public class SmsNotificationService(ISmsNotificationRepository smsNotificationRepository, ISmsNotificationSender sender, IConfiguration configuration) : ISmsNotificationService
{
    public async Task<Result> SendSmsAsync(SendSmsNotificationRequest message, CancellationToken cancellationToken)
    {
        var mailModel = new SmsNotificationModel(message.Id, DateTime.UtcNow);
        await smsNotificationRepository.AddAsync(mailModel, cancellationToken);
        
        Log.Information("Отправляем уведомление с id: {@Guid}", mailModel.Id);
        try
        {
            var attempts = configuration.GetValue<int?>("Polly:DefaultAttempts") ?? 3;
            var waitTimeInSeconds = configuration.GetValue<int?>("Polly:DefaultWaitTimeInSeconds") ?? 5;
            await GetRetryPolicy(attempts, waitTimeInSeconds, mailModel)
                .ExecuteAsync(() => sender.SendAsync(message.SenderId, message.RecipientId, message.PhoneNumber, message.Body));
            
            mailModel.SetStatus(NotificationStatus.Send);
        }
        catch (Exception ex)
        {
            Log.Warning(ex.Message);
            mailModel.SetStatus(NotificationStatus.Fail);
            await smsNotificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Fail(ex.Message);
        }

        await smsNotificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        SmsMetrics.SendNotificationCounter.Add(1, new KeyValuePair<string, object?>("sms.send.success", true));
        Log.Information("Успешно отправили уведомление с id: {@Guid}", mailModel.Id);
        return Result.Ok();
    }
    
    private AsyncRetryPolicy GetRetryPolicy(int attempts, int waitTimeInSeconds, SmsNotificationModel smsModel)
    {
        return Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                attempts,
                _ => TimeSpan.FromSeconds(waitTimeInSeconds),
                async (exception, _, retryCount, _) =>
                {
                    Log.Warning("Retry {@0RetryCount} due to {@ExceptionMessage}", retryCount, exception.Message);
                    smsModel.SetStatus(NotificationStatus.Retry);
                    SmsMetrics.SendNotificationCounter.Add(1, new KeyValuePair<string, object?>("sms.send.success", false));
                    await smsNotificationRepository.UnitOfWork.SaveChangesAsync();
                });
    }
}