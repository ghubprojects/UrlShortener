namespace UrlShortener.Application.Features.ShortUrlManagement.UseCases.GetShortUrl;

public sealed record GetShortUrlResult(
    long Id,
    string ShortCode,
    string DestinationUrl,
    bool IsEnabled,
    DateTime CreatedAt,
    DateTime? ExpiresAt);