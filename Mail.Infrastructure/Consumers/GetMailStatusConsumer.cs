using Mail.Application.Services.Interfaces;
using MassTransit;
using RabbitMqContracts.Mail.Requests;
using RabbitMqContracts.Mail.Responses;
using Serilog;

namespace Mail.Infrastructure.Consumers;

public class GetMailStatusConsumer(IStatusService statusService) : IConsumer<GetMailStatusRequest>
{
    public async Task Consume(ConsumeContext<GetMailStatusRequest> context)
    {
        Log.Information("Прочитали сообщение из брокера: {@MessageId}", context.MessageId);
        var status = await statusService.GetStatusByIdAsync(context.Message.Id, CancellationToken.None);
        if (status.IsSuccess)
        {
            Log.Information("Успешно обработали сообщение: {@MessageId}", context.MessageId);
            await context.RespondAsync(new GetMailStatusResponse((int) status.Value));
        }
        else
        {
            Log.Warning("Не обработали сообщение: {@MessageId}", context.MessageId);
        }
    }
}