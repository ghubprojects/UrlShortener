using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;
using UrlShortener.Shared.EventBus.Abstractions;
using UrlShortener.Shared.EventBus.Events;

namespace UrlShortener.Infrastructure.Messaging;

public class RedisStreamEventBus(
    IConnectionMultiplexer connection,
    ILogger<RedisStreamEventBus> logger)
    : IEventBus
{
    private readonly IDatabase _db = connection.GetDatabase();
    private readonly string _streamName = "urlshorterner_stream";

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IntegrationEvent
    {
        ArgumentNullException.ThrowIfNull(@event);

        var eventType = @event.GetType().FullName;

        var payload = JsonSerializer.Serialize(@event, @event.GetType());

        var entries = new NameValueEntry[]
        {
            new("eventId", @event.Id.ToString()),
            new("eventType", eventType),
            new("createdAt", @event.CreationDate.ToString("O")),
            new("payload", payload)
        };

        try
        {
            var messageId = await _db.StreamAddAsync(
                _streamName,
                entries,
                maxLength: 10000,
                useApproximateMaxLength: true);

            logger.LogInformation(
                "Published event {EventType} with ID {MessageId}",
                eventType,
                messageId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to publish event {EventType}",
                eventType);

            throw;
        }
    }
}
