using Core.Connections.EntityFramework.Repositories;
using Mail.Domain.Interfaces;
using Mail.Domain.Models;
using Mail.Infrastructure.DataStorage;

namespace Mail.Infrastructure.Repositories;

internal sealed class MailNotificationRepository(MailNotificationContext contextBase)
    : EfRepository<MailNotificationModel, MailNotificationContext>(contextBase), IMailNotificationRepository
{
    public override bool IsReadOnly => false;
}