using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PushConnectionLib.ConnectionServices;

namespace PushConnectionLib;

public static class Startup
{
    public static IServiceCollection TryAddPushNotificationRabbitConnectionService(this IServiceCollection services)
    {
        services.TryAddScoped<IPushConnectionService, RabbitMqConnectionService>();
        
        return services;
    }
}