using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sms.Application.Services;
using Sms.Application.Services.Interfaces;

namespace Sms.Application;

public static class ApplicationStartup
{
    public static WebApplicationBuilder AddLogic(this WebApplicationBuilder serviceCollection)
    {
        serviceCollection.Services.TryAddScoped<ISmsNotificationService, SmsNotificationService>();
        serviceCollection.Services.TryAddScoped<IStatusService, StatusService>();
        
        return serviceCollection;
    }
}
