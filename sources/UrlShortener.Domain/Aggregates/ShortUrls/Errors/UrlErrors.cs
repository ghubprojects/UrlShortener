using UrlShortener.Shared.Results;

namespace UrlShortener.Domain.Aggregates.ShortUrls.Errors;

public static class UrlErrors
{
    public static readonly Error Empty =
        new("Url.Empty", "URL cannot be empty.", ErrorType.Validation);

    public static readonly Error InvalidFormat =
        new("Url.InvalidFormat", "The URL format is invalid.", ErrorType.Validation);

    public static readonly Error InvalidScheme =
        new("Url.InvalidScheme", "Only HTTP and HTTPS URLs are allowed.", ErrorType.Validation);
}