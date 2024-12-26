using MassTransit;
using RabbitMqContracts.Mail.Requests;
using RabbitMqContracts.Mail.Responses;

namespace MailConnectionLib.ConnectionServices;

internal class RabbitMqConnectionService(IPublishEndpoint publisher, IRequestClient<GetMailStatusRequest> client) : IMailConnectionService
{
    public async Task SendNotificationAsync(SendMailNotificationRequest request)
    {
        await publisher.Publish(request);
    }

    public async Task<GetMailStatusResponse> GetMailStatusAsync(GetMailStatusRequest request)
    {
        var response = await client.GetResponse<GetMailStatusResponse>(request);
        return response.Message;
    }
}