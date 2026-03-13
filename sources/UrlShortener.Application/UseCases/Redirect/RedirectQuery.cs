using MediatR;
using UrlShortener.Shared.Results;

namespace UrlShortener.Application.UseCases.Redirect;

public sealed record RedirectQuery(
    string Code,
    string? IpAddress,
    string? UserAgent,
    string? Referer
) : IRequest<Result<string>>;
