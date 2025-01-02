using FluentResults;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Retry;
using Push.Application.Metrics;
using Push.Application.Services.Interfaces;
using Push.Domain.Interfaces;
using Push.Domain.Models;
using RabbitMqContracts.Push.Requests;
using Serilog;

namespace Push.Application.Services;

public class PushNotificationService(IPushNotificationRepository pushNotificationRepository, IPushNotificationSender sender, IConfiguration configuration) : IPushNotificationService
{
    public async Task<Result> SendPushAsync(SendPushNotificationRequest message, CancellationToken cancellationToken = default)
    {
        var mailModel = new PushNotificationModel(message.Id, DateTime.UtcNow);
        await pushNotificationRepository.AddAsync(mailModel, cancellationToken);
        
        Log.Information("Отправляем push уведомление с id: {@Guid}", mailModel.Id);
        try
        {
            var attempts = configuration.GetValue<int?>("Polly:DefaultAttempts") ?? 3;
            var waitTimeInSeconds = configuration.GetValue<int?>("Polly:DefaultWaitTimeInSeconds") ?? 5;
            await GetRetryPolicy(attempts, waitTimeInSeconds, mailModel)
                .ExecuteAsync(() => sender.SendAsync(message.DeviceToken, message.Title, message.Body));
            
            mailModel.SetStatus(NotificationStatus.Send);
        }
        catch (Exception ex)
        {
            Log.Warning(ex.Message);
            mailModel.SetStatus(NotificationStatus.Fail);
            await pushNotificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Fail(ex.Message);
        }

        await pushNotificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        PushMetrics.SendNotificationCounter.Add(1, new KeyValuePair<string, object?>("push.send.success", true));
        Log.Information("Успешно отправили уведомление с id: {@Guid}", mailModel.Id);
        return Result.Ok();
    }
    
    private AsyncRetryPolicy GetRetryPolicy(int attempts, int waitTimeInSeconds, PushNotificationModel pushModel)
    {
        return Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                attempts,
                _ => TimeSpan.FromSeconds(waitTimeInSeconds),
                async (exception, _, retryCount, _) =>
                {
                    Log.Warning("Retry {@0RetryCount} due to {@ExceptionMessage}", retryCount, exception.Message);
                    pushModel.SetStatus(NotificationStatus.Retry);
                    PushMetrics.SendNotificationCounter.Add(1, new KeyValuePair<string, object?>("push.send.success", false));
                    await pushNotificationRepository.UnitOfWork.SaveChangesAsync();
                });
    }
}