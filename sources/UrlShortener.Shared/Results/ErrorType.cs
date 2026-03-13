namespace UrlShortener.Shared.Results;

public enum ErrorType
{
    None,
    Validation,
    BusinessRuleViolation,
    NotFound,
    Conflict,
    Unauthorized,
    Forbidden,
    Unexpected
}
