using MediatR;
using UrlShortener.Shared.Results;

namespace UrlShortener.Application.UseCases.DisableShortUrl;

public record DisableShortUrlCommand(
    string Code
) : IRequest<Result>;
