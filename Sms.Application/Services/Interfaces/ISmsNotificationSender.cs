namespace Sms.Application.Services.Interfaces;

public interface ISmsNotificationSender
{
    Task SendAsync(Guid senderId, Guid recipientId, string phoneNumber, string body);
}
