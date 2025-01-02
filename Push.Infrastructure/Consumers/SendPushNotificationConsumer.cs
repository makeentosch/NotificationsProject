using MassTransit;
using Push.Application.Services.Interfaces;
using RabbitMqContracts.Push.Requests;
using Serilog;

namespace Push.Infrastructure.Consumers;

public class SendPushNotificationConsumer(IPushNotificationService pushNotificationService) : IConsumer<SendPushNotificationRequest>
{
    public async Task Consume(ConsumeContext<SendPushNotificationRequest> context)
    {
        Log.Information("Прочитали сообщение из брокера: {@MessageId}", context.MessageId);
        await pushNotificationService.SendPushAsync(context.Message);
        Log.Information("Обработали сообщение: {@MessageId}", context.MessageId);
    }
}
