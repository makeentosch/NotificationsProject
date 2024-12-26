using System.ComponentModel.DataAnnotations;
using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Connections.MassTransit;

public static class Extension
{
    public static IServiceCollection AddCustomMassTransit(
        this IServiceCollection services,
        Assembly assembly,
        IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumers(assembly);
            
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration.GetConnectionString("rabbitMq"));
                cfg.UseMessageRetry(retryConfig => AddRetryConfiguration(retryConfig, configuration));
                cfg.ConfigureEndpoints(context);
            });
        });
        
        return services;
    }

    private static void AddRetryConfiguration(IRetryConfigurator retryConfigurator, IConfiguration configuration)
    {
        var retryLimit = configuration.GetValue<int?>("MassTransit:RetryLimit") ?? 3;
        var minIntervalMilliseconds = configuration.GetValue<int?>("MassTransit:MinIntervalMilliseconds") ?? 200;
        var maxIntervalMinutes = configuration.GetValue<int?>("MassTransit:MaxIntervalMinutes") ?? 120;
        var intervalDeltaMilliseconds = configuration.GetValue<int?>("MassTransit:IntervalDeltaMilliseconds") ?? 200;

        retryConfigurator.Exponential(
            retryLimit,
            TimeSpan.FromMilliseconds(minIntervalMilliseconds),
            TimeSpan.FromMinutes(maxIntervalMinutes),
            TimeSpan.FromMilliseconds(intervalDeltaMilliseconds))
            .Ignore<ValidationException>();
    }
}
