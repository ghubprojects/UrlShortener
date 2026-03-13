using UrlShortener.Shared.Results;

namespace UrlShortener.Domain.Aggregates.ShortUrls.Errors;

public static class ShortCodeErrors
{
    public static readonly Error Required =
       new("ShortCode.Required", "Short code is required.", ErrorType.Validation);

    public static readonly Error InvalidFormat =
        new("ShortCode.InvalidFormat", "Short code must be 3–16 characters and contain only letters, numbers, or hyphens.", ErrorType.Validation);

    public static readonly Error AlreadyExists =
        new("ShortCode.AlreadyExists", "Short code already exists in the system.", ErrorType.Conflict);

    public static readonly Error Reserved =
        new("ShortCode.Reserved", "Short code is reserved.", ErrorType.BusinessRuleViolation);
}