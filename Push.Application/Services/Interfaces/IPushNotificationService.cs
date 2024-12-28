using FluentResults;
using RabbitMqContracts.Push.Requests;

namespace Push.Application.Services.Interfaces;

public interface IPushNotificationService
{
    Task<Result> SendPushAsync(SendPushNotificationRequest message, CancellationToken cancellationToken = default);
}