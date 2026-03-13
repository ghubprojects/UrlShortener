using UrlShortener.Domain.SeedWork;
using UrlShortener.Shared.Results;

namespace UrlShortener.Domain.Entities.Visits;

public class Visit : Entity<Guid>
{
    public long ShortUrlId { get; private set; }
    public string ShortCode { get; private set; } = default!;
    public DateTime VisitedAt { get; private set; }

    public string? IpAddress { get; private set; }
    public string? UserAgent { get; private set; }
    public string? Referer { get; private set; }

    private Visit() { }

    private Visit(long shortUrlId, string shortCode, DateTime visitedAt, string? ipAddress, string? userAgent, string? referer)
    {
        Id = Guid.CreateVersion7();
        ShortUrlId = shortUrlId;
        ShortCode = shortCode;
        VisitedAt = visitedAt;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        Referer = referer;
    }

    public static Result<Visit> Create(long shortUrlId, string shortCode, DateTime visitedAt, string? ipAddress, string? userAgent, string? referer)
    {
        if (shortUrlId <= 0)
            return Result.Failure<Visit>(VisitErrors.InvalidShortUrlId);

        if (string.IsNullOrWhiteSpace(shortCode))
            return Result.Failure<Visit>(VisitErrors.RequiredShortCode);

        if (visitedAt == default)
            return Result.Failure<Visit>(VisitErrors.InvalidVisitedAt);

        return new Visit(shortUrlId, shortCode, visitedAt, ipAddress, userAgent, referer);
    }
}
