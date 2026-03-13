using UrlShortener.Domain.SeedWork;

namespace UrlShortener.Domain.Events;

public record UrlVisitedDomainEvent(
    long ShortUrlId,
    string ShortCode,
    DateTime VisitedAt,
    string? IpAddress,
    string? UserAgent,
    string? Referer
) : DomainEvent;
