using MassTransit;
using RabbitMqContracts.Sms.Requests;

namespace SmsConnectionLib.ConnectionServices;

internal class RabbitMqConnectionService(IPublishEndpoint publisher) : ISmsConnectionService
{
    public async Task SendNotificationAsync(SendSmsNotificationRequest request)
    {
        await publisher.Publish(request);
    }
}