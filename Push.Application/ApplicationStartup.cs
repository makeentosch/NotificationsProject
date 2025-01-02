using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Push.Application.Services;
using Push.Application.Services.Interfaces;

namespace Push.Application;

public static class ApplicationStartup
{
    public static WebApplicationBuilder AddLogic(this WebApplicationBuilder serviceCollection)
    {
        serviceCollection.Services.TryAddScoped<IPushNotificationService, PushNotificationService>();
        serviceCollection.Services.TryAddScoped<IStatusService, StatusService>();
        
        return serviceCollection;
    }
}
