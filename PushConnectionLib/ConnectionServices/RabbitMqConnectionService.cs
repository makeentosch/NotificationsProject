using MassTransit;
using RabbitMqContracts.Push.Requests;

namespace PushConnectionLib.ConnectionServices;

internal class RabbitMqConnectionService(IPublishEndpoint publisher) : IPushConnectionService
{
    public async Task SendNotificationAsync(SendPushNotificationRequest request)
    {
        await publisher.Publish(request);
    }
}