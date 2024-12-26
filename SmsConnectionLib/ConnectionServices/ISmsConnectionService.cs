using RabbitMqContracts.Sms.Requests;

namespace SmsConnectionLib.ConnectionServices;

public interface ISmsConnectionService
{
    Task SendNotificationAsync(SendSmsNotificationRequest request);
}