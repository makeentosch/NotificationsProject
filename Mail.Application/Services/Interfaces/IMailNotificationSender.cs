using System.Net.Mail;
using RabbitMqContracts.Mail.Dtos;

namespace Mail.Application.Services.Interfaces;

public interface IMailNotificationSender
{
    Task SendAsync(Guid senderId, Guid recipientId, MailAddress recipientMail, string body, IEnumerable<AttachmentDto> attachments);
}
