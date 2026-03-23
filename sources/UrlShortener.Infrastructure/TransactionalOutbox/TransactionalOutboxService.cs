using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using UrlShortener.Application.Abstractions.TransactionalOutbox;
using UrlShortener.Shared.EventBus.Abstractions;
using UrlShortener.Shared.EventBus.Events;

namespace UrlShortener.Infrastructure.TransactionalOutbox;

public sealed class TransactionalOutboxService(
    IServiceScopeFactory scopeFactory,
    IEventBus eventBus,
    IOptions<EventBusSubscriptionInfo> options,
    ILogger<TransactionalOutboxService> logger)
    : BackgroundService
{
    private const int DelayMilliseconds = 5000;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("TransactionalOutboxService started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();

                var repository = scope.ServiceProvider.GetRequiredService<IOutboxMessageRepository>();

                var messages = await repository.GetUnprocessedMessagesAsync(stoppingToken);

                if (!messages.Any())
                {
                    await Task.Delay(DelayMilliseconds, stoppingToken);
                    continue;
                }

                foreach (var message in messages)
                {
                    try
                    {
                        var integrationEvent = RebuildEvent(message, options.Value);

                        if (integrationEvent is null)
                        {
                            logger.LogWarning("Invalid event payload {MessageId}", message.Id);

                            await repository.MarkAsFailedAsync(message, false);
                            continue;
                        }

                        await eventBus.PublishAsync(integrationEvent);

                        await repository.MarkAsProcessedAsync(message);

                        logger.LogInformation("Processed outbox message {MessageId}", message.Id);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error processing message {MessageId}", message.Id);

                        await repository.MarkAsFailedAsync(message);
                    }
                }

                await repository.SaveChangesAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // shutdown graceful
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Outbox processing failed");

                await Task.Delay(DelayMilliseconds, stoppingToken);
            }
        }

        logger.LogInformation("TransactionalOutboxService stopped");
    }

    private IntegrationEvent? RebuildEvent(OutboxMessage message, EventBusSubscriptionInfo options)
    {
        if (!options.EventTypes.TryGetValue(message.PayloadType, out var type))
        {
            logger.LogWarning("Unknown event type {PayloadType}", message.PayloadType);

            return null;
        }

        try
        {
            return JsonSerializer.Deserialize(
                message.Payload,
                type,
                options.JsonSerializerOptions) as IntegrationEvent;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Deserialization failed for {MessageId}", message.Id);

            return null;
        }
    }
}