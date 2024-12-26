using Notification.Application;
using Core.Connections.EntityFramework;
using Core.Connections.Logging;
using Core.Connections.Mapster;
using Core.Connections.MassTransit;
using Core.Connections.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Notification.Domain.Interfaces;
using Notification.Infrastructure.DataStorage;
using Notification.Infrastructure.Repositories;
using NotificationServiceDefaults;

namespace Notification.Infrastructure;

public static class InfrastructureStartup
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();
        
        builder.AddCustomSerilog(builder.Environment);
        builder.Services.AddDbContextCustom<NotificationContext>(builder.Configuration, "postgresNotifications");
        builder.Services.RegisterRepository<INotificationRepository, NotificationRepository>();
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();

        builder.Services.AddCustomSwagger();
        builder.Services.AddCustomMapster(typeof(NotificationService).Assembly);
        builder.Services.AddCustomMassTransit(typeof(InfrastructureStartup).Assembly, builder.Configuration);

        return builder;
    }
}