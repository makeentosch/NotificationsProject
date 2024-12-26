using MailConnectionLib.ConnectionServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MailConnectionLib;

public static class Startup
{
    public static IServiceCollection TryAddMailNotificationRabbitConnectionService(this IServiceCollection services)
    {
        services.TryAddScoped<IMailConnectionService, RabbitMqConnectionService>();
        
        return services;
    }
}