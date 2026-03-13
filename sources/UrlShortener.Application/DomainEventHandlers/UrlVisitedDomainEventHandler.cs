using MediatR;
using Microsoft.Extensions.Logging;
using UrlShortener.Domain.Entities.Visits;
using UrlShortener.Domain.Events;

namespace UrlShortener.Application.DomainEventHandlers;

public class UrlVisitedDomainEventHandler(
    IVisitRepository repository,
    ILogger<UrlVisitedDomainEventHandler> logger) : INotificationHandler<UrlVisitedDomainEvent>
{
    public async Task Handle(UrlVisitedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var visitResult = Visit.Create(
            domainEvent.ShortUrlId,
            domainEvent.ShortCode,
            domainEvent.VisitedAt,
            domainEvent.IpAddress,
            domainEvent.UserAgent,
            domainEvent.Referer);

        if (visitResult.IsFailure)
        {
            logger.LogError("Failed to create visit for short URL with ID {ShortUrlId}: {Errors}",
                domainEvent.ShortUrlId, 
                visitResult.Error.Message);

            return;
        }

        await repository.AddAsync(visitResult.Value, cancellationToken);
    }
}
