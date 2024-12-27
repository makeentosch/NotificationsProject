using Mail.Application.Services.Interfaces;
using MassTransit;
using RabbitMqContracts.Mail.Requests;
using Serilog;

namespace Mail.Infrastructure.Consumers;

public class SendMailNotificationConsumer(IMailNotificationService mailNotificationService) : IConsumer<SendMailNotificationRequest>
{
    public async Task Consume(ConsumeContext<SendMailNotificationRequest> context)
    {
        Log.Information("Прочитали сообщение из брокера: {@MessageId}", context.MessageId);
        await mailNotificationService.SendMailAsync(context.Message);
        Log.Information("Обработали сообщение: {@MessageId}", context.MessageId);
    }
}
