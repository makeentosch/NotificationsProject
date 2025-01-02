using Core.Connections.EntityFramework.Repositories;
using Sms.Domain.Interfaces;
using Sms.Domain.Models;
using Sms.Infrastructure.DataStorage;

namespace Sms.Infrastructure.Repositories;

internal sealed class SmsNotificationRepository(SmsNotificationContext contextBase)
    : EfRepository<SmsNotificationModel, SmsNotificationContext>(contextBase), ISmsNotificationRepository
{
    public override bool IsReadOnly => false;
}