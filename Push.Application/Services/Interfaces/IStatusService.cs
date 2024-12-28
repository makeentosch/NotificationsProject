using FluentResults;
using Push.Domain.Models;

namespace Push.Application.Services.Interfaces;

public interface IStatusService
{
    Task<Result<NotificationStatus>> GetStatusByIdAsync(Guid notificationId, CancellationToken cancellationToken);
}