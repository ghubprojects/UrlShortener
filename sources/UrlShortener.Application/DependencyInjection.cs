using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UrlShortener.Application.Behaviors;

namespace UrlShortener.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Add MediatR with all handlers from this assembly
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(CachingBehavior<,>));
            config.AddOpenBehavior(typeof(CacheInvalidationBehavior<,>));
        });

        return services;
    }
}
