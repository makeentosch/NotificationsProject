using MailConnectionLib;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Notification.Application.Interfaces;
using PushConnectionLib;
using SmsConnectionLib;

namespace Notification.Application;

public static class ApplicationStartup
{
    public static WebApplicationBuilder AddLogic(this WebApplicationBuilder serviceCollection)
    {
        serviceCollection.Services.TryAddScoped<INotificationService, NotificationService>();
        serviceCollection.Services.TryAddMailNotificationRabbitConnectionService();
        serviceCollection.Services.TryAddSmsNotificationRabbitConnectionService();
        serviceCollection.Services.TryAddPushNotificationRabbitConnectionService();
        
        return serviceCollection;
    }
}
