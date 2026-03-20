using UrlShortener.Shared.EventBus.Events;

namespace UrlShortener.Application.IntegrationEvents;

public sealed record UrlVisitedIntegrationEvent(
    long ShortUrlId,
    string ShortCode,
    DateTime VisitedAt,
    string? IpAddress,
    string? UserAgent,
    string? Referer
) : IntegrationEvent;