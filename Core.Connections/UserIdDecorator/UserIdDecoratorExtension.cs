using Core.Connections.UserIdDecorator.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core.Connections.UserIdDecorator;

public static class UserIdDecoratorExtension
{
    public static IServiceCollection TryAddHttpUserDecorator(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.TryAddScoped<IUserIdDecorator, HttpUserIdDecorator>();
        
        return services;
    }
}