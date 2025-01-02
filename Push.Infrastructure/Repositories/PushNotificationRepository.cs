using Core.Connections.EntityFramework.Repositories;
using Push.Domain.Interfaces;
using Push.Domain.Models;
using Push.Infrastructure.DataStorage;

namespace Push.Infrastructure.Repositories;

internal sealed class PushNotificationRepository(PushNotificationContext contextBase)
    : EfRepository<PushNotificationModel, PushNotificationContext>(contextBase), IPushNotificationRepository
{
    public override bool IsReadOnly => false;
}