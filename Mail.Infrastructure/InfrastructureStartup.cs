using Core.Connections.EntityFramework;
using Core.Connections.Logging;
using Core.Connections.MassTransit;
using Core.Connections.Swagger;
using Mail.Application.Services.Interfaces;
using Mail.Domain.Interfaces;
using Mail.Infrastructure.DataStorage;
using Mail.Infrastructure.NotificationSender;
using Mail.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NotificationServiceDefaults;

namespace Mail.Infrastructure;

public static class InfrastructureStartup
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();
        
        builder.AddCustomSerilog(builder.Environment);
        builder.Services.AddDbContextCustom<MailNotificationContext>(builder.Configuration, "postgresMails");
        builder.Services.RegisterRepository<IMailNotificationRepository, MailNotificationRepository>();

        builder.Services.AddScoped<IMailNotificationSender, MailNotificationSender>();
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();

        builder.Services.AddCustomSwagger();
        builder.Services.AddCustomMassTransit(typeof(InfrastructureStartup).Assembly, builder.Configuration);
        
        return builder;
    }
}