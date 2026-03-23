using MediatR;
using UrlShortener.Shared.Results;

namespace UrlShortener.Application.Features.ShortUrlManagement.UseCases.DisableShortUrl;

public record DisableShortUrlCommand(
    string Code
) : IRequest<Result>;
