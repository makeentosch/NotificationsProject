namespace RabbitMqContracts.Sms.Requests;

public record SendSmsNotificationRequest(Guid Id, Guid SenderId, Guid RecipientId, string PhoneNumber, string Body);