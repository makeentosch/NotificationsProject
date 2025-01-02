using FluentResults;
using RabbitMqContracts.Mail.Requests;

namespace Mail.Application.Services.Interfaces;

public interface IMailNotificationService
{
    Task<Result> SendMailAsync(SendMailNotificationRequest message, CancellationToken cancellationToken = default);
}