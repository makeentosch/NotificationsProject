using RabbitMqContracts.Mail.Dtos;

namespace RabbitMqContracts.Mail.Requests;

public record SendMailNotificationRequest(Guid Id, string Message, IEnumerable<AttachmentDto> Attachments, Guid RecipientId, string RecipientMail, Guid SenderId);