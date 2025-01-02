using MassTransit;
using RabbitMqContracts.Push.Requests;
using RabbitMqContracts.Push.Responses;

namespace PushConnectionLib.ConnectionServices;

internal class RabbitMqConnectionService(IPublishEndpoint publisher, IRequestClient<GetPushStatusRequest> client) : IPushConnectionService
{
    public async Task SendNotificationAsync(SendPushNotificationRequest request)
    {
        await publisher.Publish(request);
    }

    public async Task<GetPushStatusResponse> GetPushStatusAsync(GetPushStatusRequest request)
    {
        var response = await client.GetResponse<GetPushStatusResponse>(request);
        return response.Message;
    }
}