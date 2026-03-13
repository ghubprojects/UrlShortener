using UrlShortener.Shared.Results;

namespace UrlShortener.Domain.Aggregates.ShortUrls.Errors;

public static class ShortUrlErrors
{
    public static readonly Error ExpiresAtInPast =
        new("ShortUrl.ExpiresAtInPast", "Expiration time cannot be in the past.", ErrorType.Validation);

    public static readonly Error InvalidExpiration =
        new("ShortUrl.InvalidExpiration", "Expiration time must be greater than the creation time.", ErrorType.Validation);

    public static readonly Error Expired =
         new("ShortUrl.Expired", "Short URL has expired and can no longer be used.", ErrorType.BusinessRuleViolation);

    public static readonly Error CannotRedirect =
        new("ShortUrl.CannotRedirect", "Short URL cannot redirect because it is either disabled or expired.", ErrorType.BusinessRuleViolation);

    public static readonly Error AlreadyDisabled =
        new("ShortUrl.AlreadyDisabled", "Short URL is already disabled.", ErrorType.Conflict);

    public static readonly Error AlreadyEnabled =
        new("ShortUrl.AlreadyEnabled", "Short URL is already enabled.", ErrorType.Conflict);

    public static readonly Error NotFound =
        new("ShortUrl.NotFound", "Short URL was not found", ErrorType.NotFound);
}