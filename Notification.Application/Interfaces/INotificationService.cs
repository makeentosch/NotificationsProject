using Notification.Domain.Models;

namespace Notification.Application.Interfaces;

public interface INotificationService
{
    Task SendNotificationAsync(NotificationModel model, CancellationToken cancellationToken = default);

    Task<NotificationModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<NotificationModel>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Dictionary<int, int>> GetStatusesByIdAsync(Guid id, List<NotificationType> types, CancellationToken cancellationToken = default);
}