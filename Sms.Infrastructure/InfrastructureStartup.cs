using Core.Connections.EntityFramework;
using Core.Connections.Logging;
using Core.Connections.MassTransit;
using Core.Connections.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NotificationServiceDefaults;
using Sms.Application.Services.Interfaces;
using Sms.Domain.Interfaces;
using Sms.Infrastructure.DataStorage;
using Sms.Infrastructure.NotificationSender;
using Sms.Infrastructure.Repositories;

namespace Sms.Infrastructure;

public static class InfrastructureStartup
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();
        
        builder.AddCustomSerilog(builder.Environment);
        builder.Services.AddDbContextCustom<SmsNotificationContext>(builder.Configuration, "postgresSms");
        builder.Services.RegisterRepository<ISmsNotificationRepository, SmsNotificationRepository>();

        builder.Services.AddScoped<ISmsNotificationSender, SmsNotificationSender>();
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();

        builder.Services.AddCustomSwagger();
        builder.Services.AddCustomMassTransit(typeof(InfrastructureStartup).Assembly, builder.Configuration);
        
        return builder;
    }
}