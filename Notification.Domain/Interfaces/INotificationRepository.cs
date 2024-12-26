using Core.Domain.Repositories.Interfaces;
using Notification.Domain.Models;

namespace Notification.Domain.Interfaces;

public interface INotificationRepository : IRepository<NotificationModel>;