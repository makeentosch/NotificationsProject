using Core.Connections.EntityFramework.Repositories;
using Notification.Domain.Interfaces;
using Notification.Domain.Models;
using Notification.Infrastructure.DataStorage;

namespace Notification.Infrastructure.Repositories;

internal sealed class NotificationRepository(NotificationContext contextBase)
    : EfRepository<NotificationModel, NotificationContext>(contextBase), INotificationRepository
{
    public override bool IsReadOnly => false;
}