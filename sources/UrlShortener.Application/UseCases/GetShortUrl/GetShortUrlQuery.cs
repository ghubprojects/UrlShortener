using MediatR;
using UrlShortener.Shared.Results;

namespace UrlShortener.Application.UseCases.GetShortUrl;

public sealed record GetShortUrlQuery(
    string Code
) : IRequest<Result<GetShortUrlResult?>>;
