using RabbitMqContracts.Push.Requests;

namespace PushConnectionLib.ConnectionServices;

public interface IPushConnectionService
{
    Task SendNotificationAsync(SendPushNotificationRequest request);
}