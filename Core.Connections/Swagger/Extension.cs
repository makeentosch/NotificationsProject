using Microsoft.Extensions.DependencyInjection;

namespace Core.Connections.Swagger;

public static class Extension
{
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        { });

        return services;
    }
}