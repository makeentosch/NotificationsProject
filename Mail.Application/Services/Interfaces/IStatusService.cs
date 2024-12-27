using FluentResults;
using Mail.Domain.Models;

namespace Mail.Application.Services.Interfaces;

public interface IStatusService
{
    Task<Result<NotificationStatus>> GetStatusByIdAsync(Guid notificationId, CancellationToken cancellationToken);
}