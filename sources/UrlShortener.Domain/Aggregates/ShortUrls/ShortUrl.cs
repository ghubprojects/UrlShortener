using UrlShortener.Domain.Aggregates.ShortUrls.Errors;
using UrlShortener.Domain.Events;
using UrlShortener.Domain.SeedWork;
using UrlShortener.Shared.Results;

namespace UrlShortener.Domain.Aggregates.ShortUrls;

public class ShortUrl : AggregateRoot<long>
{
    public ShortCode ShortCode { get; private set; } = default!;
    public Url DestinationUrl { get; private set; } = default!;
    public bool IsEnabled { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ExpiresAt { get; private set; }

    private ShortUrl() { }

    private ShortUrl(long id, ShortCode shortCode, Url destinationUrl, DateTime now, DateTime? expiresAt)
    {
        Id = id;
        ShortCode = shortCode;
        DestinationUrl = destinationUrl;
        IsEnabled = true;
        CreatedAt = now;
        ExpiresAt = expiresAt;
    }

    #region Factory Methods

    public static Result<ShortUrl> Create(long id, ShortCode shortCode, Url destinationUrl, DateTime now, DateTime? expiresAt = null)
    {
        if (expiresAt.HasValue && expiresAt <= now)
            return Result<ShortUrl>.Failure(ShortUrlErrors.ExpiresAtInPast);

        return new ShortUrl(id, shortCode, destinationUrl, now, expiresAt);
    }

    #endregion

    #region Behavior Methods

    public Result RecordVisit(DateTime now, string? ipAddress, string? userAgent, string? referer)
    {
        if (!CanRedirect(now))
            return Result.Failure(ShortUrlErrors.CannotRedirect);

        AddDomainEvent(new UrlVisitedDomainEvent(
            Id,
            ShortCode.Value,
            now,
            ipAddress,
            userAgent,
            referer
        ));

        return Result.Success();
    }

    public Result UpdateDestinationUrl(Url newUrl, DateTime now)
    {
        if (!CanRedirect(now))
            return Result.Failure(ShortUrlErrors.CannotRedirect);

        if (newUrl == DestinationUrl)
            return Result.Success();

        DestinationUrl = newUrl;

        return Result.Success();
    }

    public Result Disable()
    {
        if (!IsEnabled)
            return Result.Failure(ShortUrlErrors.AlreadyDisabled);

        IsEnabled = false;

        return Result.Success();
    }

    public Result Enable(DateTime now)
    {
        if (IsEnabled)
            return Result.Failure(ShortUrlErrors.AlreadyEnabled);

        if (IsExpired(now))
            return Result.Failure(ShortUrlErrors.Expired);

        IsEnabled = true;

        return Result.Success();
    }

    public Result SetExpiresAt(DateTime now, DateTime? expiresAt)
    {
        if (expiresAt.HasValue && expiresAt <= now)
            return Result.Failure(ShortUrlErrors.ExpiresAtInPast);

        if (expiresAt.HasValue && expiresAt <= CreatedAt)
            return Result.Failure(ShortUrlErrors.InvalidExpiration);

        ExpiresAt = expiresAt;

        return Result.Success();
    }

    #endregion

    #region Status Check Methods

    public bool IsExpired(DateTime now)
        => ExpiresAt.HasValue && ExpiresAt <= now;

    public bool CanRedirect(DateTime now)
        => IsEnabled && !IsExpired(now);

    #endregion
}
