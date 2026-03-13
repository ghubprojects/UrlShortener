using MediatR;
using UrlShortener.Domain.Aggregates.ShortUrls;
using UrlShortener.Domain.Aggregates.ShortUrls.Errors;
using UrlShortener.Shared.Results;

namespace UrlShortener.Application.UseCases.GetShortUrl;

public sealed class GetShortUrlQueryHandler(IShortUrlRepository repository)
    : IRequestHandler<GetShortUrlQuery, Result<GetShortUrlResult?>>
{
    public async Task<Result<GetShortUrlResult?>> Handle(GetShortUrlQuery query, CancellationToken cancellationToken)
    {
        var codeResult = ShortCode.Create(query.Code);
        if (codeResult.IsFailure)
            return codeResult.ToFailure<GetShortUrlResult?>();

        var shortUrl = await repository.GetByCodeAsync(codeResult.Value, cancellationToken);
        if (shortUrl is null)
            return Result<GetShortUrlResult?>.Failure(ShortUrlErrors.NotFound);

        return new GetShortUrlResult(
            shortUrl.Id,
            shortUrl.ShortCode.Value,
            shortUrl.DestinationUrl.Value,
            shortUrl.IsEnabled,
            shortUrl.CreatedAt,
            shortUrl.ExpiresAt
        );
    }
}