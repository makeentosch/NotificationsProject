using Core.Domain.Repositories.Interfaces;
using Push.Domain.Models;

namespace Push.Domain.Interfaces;

public interface IPushNotificationRepository : IRepository<PushNotificationModel>;