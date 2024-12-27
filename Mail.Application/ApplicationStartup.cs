using Mail.Application.Services;
using Mail.Application.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Mail.Application;

public static class ApplicationStartup
{
    public static WebApplicationBuilder AddLogic(this WebApplicationBuilder serviceCollection)
    {
        serviceCollection.Services.TryAddScoped<IMailNotificationService, MailNotificationService>();
        serviceCollection.Services.TryAddScoped<IStatusService, StatusService>();
        
        return serviceCollection;
    }
}
