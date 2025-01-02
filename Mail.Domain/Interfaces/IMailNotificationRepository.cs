using Core.Domain.Repositories.Interfaces;
using Mail.Domain.Models;

namespace Mail.Domain.Interfaces;

public interface IMailNotificationRepository : IRepository<MailNotificationModel>;