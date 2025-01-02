using RabbitMqContracts.Push.Requests;
using RabbitMqContracts.Push.Responses;

namespace PushConnectionLib.ConnectionServices;

public interface IPushConnectionService
{
    Task SendNotificationAsync(SendPushNotificationRequest request);
        
    Task<GetPushStatusResponse> GetPushStatusAsync(GetPushStatusRequest request);
}