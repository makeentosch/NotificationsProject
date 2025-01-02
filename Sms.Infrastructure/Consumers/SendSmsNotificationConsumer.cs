using MassTransit;
using RabbitMqContracts.Sms.Requests;
using Serilog;
using Sms.Application.Services.Interfaces;

namespace Sms.Infrastructure.Consumers;

public class SendSmsNotificationConsumer(ISmsNotificationService smsNotificationService) : IConsumer<SendSmsNotificationRequest>
{
    public async Task Consume(ConsumeContext<SendSmsNotificationRequest> context)
    {
        Log.Information("Прочитали сообщение из брокера: {@MessageId}", context.MessageId);
        await smsNotificationService.SendSmsAsync(context.Message);
        Log.Information("Обработали сообщение: {@MessageId}", context.MessageId);
    }
}
