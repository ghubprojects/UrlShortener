using MediatR;
using UrlShortener.Shared.Results;

namespace UrlShortener.Application.Features.ShortUrlManagement.UseCases.CreateShortUrl;

public sealed record CreateShortUrlCommand(
    string DestinationUrl,
    string? CustomAlias,
    DateTime? ExpiredAt
) : IRequest<Result<CreateShortUrlResult>>;