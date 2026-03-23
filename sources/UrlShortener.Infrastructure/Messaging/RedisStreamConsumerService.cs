using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;
using UrlShortener.Shared.EventBus.Abstractions;
using UrlShortener.Shared.EventBus.Events;

namespace UrlShortener.Infrastructure.Messaging;

public sealed class RedisStreamConsumerService : BackgroundService
{
    private const string ConsumerGroup = "urlshortener_group";
    private readonly IDatabase _db;
    private readonly ILogger<RedisStreamConsumerService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly EventBusSubscriptionInfo _subscriptionInfo;
    private readonly string _consumerName;

    private readonly string _streamName = "urlshorterner_stream";

    public RedisStreamConsumerService(
        IConnectionMultiplexer connection,
        IServiceScopeFactory scopeFactory,
        IOptions<EventBusSubscriptionInfo> options,
        ILogger<RedisStreamConsumerService> logger)
    {
        _db = connection.GetDatabase();
        _scopeFactory = scopeFactory;
        _subscriptionInfo = options.Value;
        _logger = logger;

        _consumerName = Environment.MachineName + "-" + Guid.NewGuid().ToString("N");

        // ensure consumer group exists
        try
        {
            _db.StreamCreateConsumerGroup(_streamName, ConsumerGroup, "$");
        }
        catch (RedisServerException ex) when (ex.Message.Contains("BUSYGROUP"))
        {
            // group already exists
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("RedisStreamConsumerService started as consumer {Consumer}", _consumerName);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var entries = await _db.StreamReadGroupAsync(
                    _streamName,
                    ConsumerGroup,
                    _consumerName,
                    ">", // read new messages
                    count: 10);

                if (entries.Length == 0)
                {
                    await Task.Delay(500, stoppingToken);
                    continue;
                }

                foreach (var entry in entries)
                {
                    try
                    {
                        var payload = entry["payload"];
                        var eventTypeName = entry["eventType"];

                        if (!_subscriptionInfo.EventTypes.TryGetValue(eventTypeName, out var type))
                        {
                            _logger.LogWarning("Unknown event type {EventType}", eventTypeName);
                            await _db.StreamAcknowledgeAsync(_streamName, ConsumerGroup, entry.Id);
                            continue;
                        }

                        var integrationEvent = JsonSerializer.Deserialize(payload, type, _subscriptionInfo.JsonSerializerOptions) as IntegrationEvent;
                        if (integrationEvent is null)
                        {
                            _logger.LogWarning("Failed to deserialize event {EventId}", entry["eventId"]);
                            await _db.StreamAcknowledgeAsync(_streamName, ConsumerGroup, entry.Id);
                            continue;
                        }

                        using var scope = _scopeFactory.CreateScope();

                        // Get all the handlers using the event type as the key
                        foreach (var handler in scope.ServiceProvider.GetKeyedServices<IIntegrationEventHandler>(type))
                        {
                            await handler.Handle(integrationEvent);
                        }

                        await _db.StreamAcknowledgeAsync(_streamName, ConsumerGroup, entry.Id);
                        _logger.LogInformation("Processed event {EventType} with ID {EventId}", eventTypeName, entry["eventId"]);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing Redis stream entry {EntryId}", entry.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading from Redis stream");
                await Task.Delay(1000, stoppingToken);
            }
        }

        _logger.LogInformation("RedisStreamConsumerService stopped");
    }
}