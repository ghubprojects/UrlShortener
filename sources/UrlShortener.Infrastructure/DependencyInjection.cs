using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using UrlShortener.Application.Abstractions.Caching;
using UrlShortener.Application.Abstractions.IdProcesser;
using UrlShortener.Application.Abstractions.TransactionalOutbox;
using UrlShortener.Application.IntegrationEvents;
using UrlShortener.Domain.Aggregates.ShortUrls;
using UrlShortener.Domain.Entities.Visits;
using UrlShortener.Infrastructure.Caching;
using UrlShortener.Infrastructure.Encoding;
using UrlShortener.Infrastructure.IdProcesser;
using UrlShortener.Infrastructure.Messaging;
using UrlShortener.Infrastructure.Options;
using UrlShortener.Infrastructure.Persistence.DataContext;
using UrlShortener.Infrastructure.Persistence.Interceptors;
using UrlShortener.Infrastructure.Persistence.Repositories;
using UrlShortener.Infrastructure.TransactionalOutbox;
using UrlShortener.Shared.EventBus.Abstractions;
using UrlShortener.Shared.EventBus.Extensions;

namespace UrlShortener.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        // Add options
        services.Configure<OptimusOptions>(configuration.GetSection(OptimusOptions.SectionName));

        // Add database
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString("urlshortenerdb"))
                .UseSnakeCaseNamingConvention();

            foreach (var interceptor in serviceProvider.GetServices<SaveChangesInterceptor>())
                options.AddInterceptors(interceptor);
        });

        // Add interceptors
        services.AddScoped<SaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        builder.EnrichNpgsqlDbContext<AppDbContext>();

        // Add caching
        builder.AddRedisClient("redis");
        services.AddSingleton<ICacheService, RedisCacheService>();

        // Add repositories
        services.AddScoped<IShortUrlRepository, ShortUrlRepository>();
        services.AddScoped<IVisitRepository, VisitRepository>();
        services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();

        // Add ID processing services
        services.AddSingleton<IIdGenerator, IdGenerator>();
        services.AddSingleton<IIdEncoder, IdEncoder>();

        // Add event bus and integration event handlers
        services.AddEventBus()
            .AddSubscription<UrlVisitedIntegrationEvent, UrlVisitedIntegrationEventHandler>();

        // Add obfuscator
        services.AddSingleton<OptimusObfuscator>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<OptimusOptions>>().Value;
            return new(options.Prime, options.Inverse, options.Random, options.BitLength);
        });

        return builder;
    }

    private static EventBusBuilder AddEventBus(this IServiceCollection services)
    {
        services.AddSingleton<IEventBus, RedisStreamEventBus>();

        services.AddHostedService<TransactionalOutboxService>();
        services.AddHostedService<RedisStreamConsumerService>();

        return new EventBusBuilder(services);
    }

    private class EventBusBuilder(IServiceCollection services) : IEventBusBuilder
    {
        public IServiceCollection Services => services;
    }
}
