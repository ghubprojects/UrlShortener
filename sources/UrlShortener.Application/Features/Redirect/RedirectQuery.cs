using MediatR;
using UrlShortener.Application.Abstractions.Caching;
using UrlShortener.Shared.Results;

namespace UrlShortener.Application.Features.Redirect;

public sealed record RedirectQuery(
    string Code,
    string? IpAddress,
    string? UserAgent,
    string? Referer
) : IRequest<Result<string>>, ICacheableQuery<Result<string>>
{
    public string CacheKey => $"url:{Code}";
    public TimeSpan? Expiration => TimeSpan.FromMinutes(30);
}
