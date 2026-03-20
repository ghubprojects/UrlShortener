using StackExchange.Redis;
using System.Text.Json;
using UrlShortener.Shared.EventBus.Abstractions;
using UrlShortener.Shared.EventBus.Events;

namespace UrlShortener.Infrastructure.Messaging;

public class RedisStreamEventBus(IConnectionMultiplexer muxer) : IEventBus
{
    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IntegrationEvent
    {
        var db = muxer.GetDatabase();
        var json = JsonSerializer.Serialize(@event);
        await db.StreamAddAsync("urlshorterner", "data", json);
    }
}
