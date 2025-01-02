using MassTransit;
using RabbitMqContracts.Sms.Requests;
using RabbitMqContracts.Sms.Responses;

namespace SmsConnectionLib.ConnectionServices;

internal class RabbitMqConnectionService(IPublishEndpoint publisher, IRequestClient<GetSmsStatusRequest> client) : ISmsConnectionService
{
    public async Task SendNotificationAsync(SendSmsNotificationRequest request)
    {
        await publisher.Publish(request);
    }

    public async Task<GetSmsStatusResponse> GetSmsStatusAsync(GetSmsStatusRequest request)
    {
        var response = await client.GetResponse<GetSmsStatusResponse>(request);
        return response.Message;
    }
}