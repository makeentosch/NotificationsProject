using Core.Connections.EntityFramework;
using Core.Connections.Logging;
using Core.Connections.MassTransit;
using Core.Connections.Swagger;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NotificationServiceDefaults;
using Push.Application.Services.Interfaces;
using Push.Domain.Interfaces;
using Push.Infrastructure.DataStorage;
using Push.Infrastructure.NotificationSender;
using Push.Infrastructure.Repositories;

namespace Push.Infrastructure;

public static class InfrastructureStartup
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();
        
        builder.AddCustomSerilog(builder.Environment);
        builder.Services.AddDbContextCustom<PushNotificationContext>(builder.Configuration, "postgresPush");
        builder.Services.RegisterRepository<IPushNotificationRepository, PushNotificationRepository>();

        builder.Services.AddScoped<IPushNotificationSender, PushNotificationSender>();
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();
        
        FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromFile("firebase.json"),
        });

        builder.Services.AddCustomSwagger();
        builder.Services.AddCustomMassTransit(typeof(InfrastructureStartup).Assembly, builder.Configuration);
        
        return builder;
    }
}