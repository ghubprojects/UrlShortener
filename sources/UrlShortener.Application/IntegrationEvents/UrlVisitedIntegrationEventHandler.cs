using Microsoft.Extensions.Logging;
using UrlShortener.Domain.Entities.Visits;
using UrlShortener.Shared.EventBus.Abstractions;

namespace UrlShortener.Application.IntegrationEvents;

public class UrlVisitedIntegrationEventHandler(
    IVisitRepository repository,
    ILogger<UrlVisitedIntegrationEventHandler> logger)
    : IIntegrationEventHandler<UrlVisitedIntegrationEvent>
{
    public async Task Handle(UrlVisitedIntegrationEvent integrationEvent)
    {
        var visitResult = Visit.Create(
            integrationEvent.ShortUrlId,
            integrationEvent.ShortCode,
            integrationEvent.VisitedAt,
            integrationEvent.IpAddress,
            integrationEvent.UserAgent,
            integrationEvent.Referer);

        if (visitResult.IsFailure)
        {
            logger.LogError("Failed to create visit for short URL with ID {ShortUrlId}: {Errors}",
                integrationEvent.ShortUrlId,
                visitResult.Error.Message);

            return;
        }

        await repository.AddAsync(visitResult.Value);

        await repository.SaveChangesAsync();
    }
}