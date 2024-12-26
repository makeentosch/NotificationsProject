namespace RabbitMqContracts.Push.Requests;

public record SendPushNotificationRequest(Guid Id, string Title, string DeviceToken, string Body);