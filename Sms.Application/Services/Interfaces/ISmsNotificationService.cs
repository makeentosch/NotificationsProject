using FluentResults;
using RabbitMqContracts.Sms.Requests;

namespace Sms.Application.Services.Interfaces;

public interface ISmsNotificationService
{
    Task<Result> SendSmsAsync(SendSmsNotificationRequest message, CancellationToken cancellationToken = default);
}