using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SmsConnectionLib.ConnectionServices;

namespace SmsConnectionLib;

public static class Startup
{
    public static IServiceCollection TryAddSmsNotificationRabbitConnectionService(this IServiceCollection services)
    {
        services.TryAddScoped<ISmsConnectionService, RabbitMqConnectionService>();
        
        return services;
    }
}