using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using UrlShortener.Application.Abstractions.TransactionalOutbox;
using UrlShortener.Shared.EventBus.Abstractions;
using UrlShortener.Shared.EventBus.Events;

namespace UrlShortener.Infrastructure.TransactionalOutbox;

public sealed class TransactionalOutboxService(
    IOutboxMessageRepository repository,
    IEventBus eventPublisher,
    IOptions<EventBusSubscriptionInfo> subscriptionOptions,
    ILogger<TransactionalOutboxService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting TransactionOutbox polling service...");

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var messages = await repository.GetUnprocessedMessagesAsync(cancellationToken);

                foreach (var message in messages)
                {
                    try
                    {
                        var @event = RebuildEvent(message);

                        if (@event == null)
                        {
                            logger.LogWarning("Failed to rebuild event from message {MessageId}", message.Id);
                            await repository.MarkAsFailedAsync(message, false); // mark as failed without retry
                            continue;
                        }

                        logger.LogInformation("Publish event from  publisher: {m}", message.Payload);
                        await eventPublisher.PublishAsync(@event);
                        await repository.MarkAsProcessedAsync(message);
                        await repository.SaveChangesAsync(cancellationToken);

                        logger.LogInformation("Published message {MessageId}", message.Id);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Failed to publish message {MessageId}", message.Id);
                        await repository.MarkAsFailedAsync(message);
                    }
                }

                if (!messages.Any() && !cancellationToken.IsCancellationRequested)
                    await Task.Delay(5000, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing TransactionOutboxService");
            }
        }
    }

    private IntegrationEvent? RebuildEvent(OutboxMessage message)
    {
        if (!subscriptionOptions.Value.EventTypes.TryGetValue(message.PayloadType, out var eventType))
        {
            logger.LogWarning("Unable to resolve event type for event name {PayloadType}", message.PayloadType);
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize(message.Payload, eventType, subscriptionOptions.Value.JsonSerializerOptions) as IntegrationEvent;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to deserialize message {MessageId} with type {PayloadType}", message.Id, message.PayloadType);
            return null;
        }
    }
}