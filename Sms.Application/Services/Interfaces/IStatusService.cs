using FluentResults;
using Sms.Domain.Models;

namespace Sms.Application.Services.Interfaces;

public interface IStatusService
{
    Task<Result<NotificationStatus>> GetStatusByIdAsync(Guid notificationId, CancellationToken cancellationToken);
}