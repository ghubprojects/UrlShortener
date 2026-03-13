namespace UrlShortener.Application.UseCases.CreateShortUrl;

public sealed record CreateShortUrlResult(
    long Id,
    string Code,
    string OriginalUrl,
    DateTime CreatedAt,
    DateTime? ExpiredAt,
    bool IsActive);
