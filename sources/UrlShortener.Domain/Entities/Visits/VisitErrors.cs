using UrlShortener.Shared.Results;

namespace UrlShortener.Domain.Entities.Visits;

public static class VisitErrors
{
    public static readonly Error InvalidShortUrlId =
       new("Visit.InvalidShortUrlId", "Short URL identifier is invalid.", ErrorType.Validation);

    public static readonly Error RequiredShortCode =
        new("Visit.RequiredShortCode", "Short code is required for a visit.", ErrorType.Validation);

    public static readonly Error InvalidVisitedAt =
        new("Visit.InvalidVisitedAt", "Visited time is invalid.", ErrorType.Validation);
}