using MediatR;
using System.Text.Json;
using UrlShortener.Application.Abstractions.TransactionalOutbox;
using UrlShortener.Application.IntegrationEvents;
using UrlShortener.Domain.Events;

namespace UrlShortener.Application.DomainEventHandlers;

public class UrlVisitedDomainEventHandler(IOutboxMessageRepository repository)
    : INotificationHandler<UrlVisitedDomainEvent>
{
    public async Task Handle(UrlVisitedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var integrationEvent = new UrlVisitedIntegrationEvent(
            domainEvent.ShortUrlId,
            domainEvent.ShortCode,
            domainEvent.VisitedAt,
            domainEvent.IpAddress,
            domainEvent.UserAgent,
            domainEvent.Referer);

        var message = new OutboxMessage(
            JsonSerializer.Serialize(integrationEvent),
            integrationEvent.GetType().FullName!);

        await repository.AddAsync(message, cancellationToken);
    }
}
