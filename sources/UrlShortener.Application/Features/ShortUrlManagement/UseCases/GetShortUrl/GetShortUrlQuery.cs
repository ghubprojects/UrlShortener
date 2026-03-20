using MediatR;
using UrlShortener.Shared.Results;

namespace UrlShortener.Application.Features.ShortUrlManagement.UseCases.GetShortUrl;

public sealed record GetShortUrlQuery(
    string Code
) : IRequest<Result<GetShortUrlResult?>>;
