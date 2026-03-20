using MediatR;
using UrlShortener.Application.Abstractions.IdProcesser;
using UrlShortener.Domain.Aggregates.ShortUrls;
using UrlShortener.Domain.Aggregates.ShortUrls.Errors;
using UrlShortener.Shared.Results;

namespace UrlShortener.Application.Features.ShortUrlManagement.UseCases.CreateShortUrl;

public sealed class CreateShortUrlCommandHandler(
    IShortUrlRepository repository,
    IIdGenerator idGenerator,
    IIdEncoder idEncoder)
    : IRequestHandler<CreateShortUrlCommand, Result<CreateShortUrlResult>>
{
    public async Task<Result<CreateShortUrlResult>> Handle(CreateShortUrlCommand command, CancellationToken cancellationToken)
    {
        var id = idGenerator.Generate();

        var shortCodeResult = await CreateShortCode(command, id, cancellationToken);
        if (shortCodeResult.IsFailure)
            return shortCodeResult.ToFailure<CreateShortUrlResult>();

        var urlResult = Url.Create(command.DestinationUrl);
        if (urlResult.IsFailure)
            return urlResult.ToFailure<CreateShortUrlResult>();

        var shortUrlResult = ShortUrl.Create(id, shortCodeResult.Value, urlResult.Value, DateTime.UtcNow, command.ExpiredAt);
        if (shortUrlResult.IsFailure)
            return shortUrlResult.ToFailure<CreateShortUrlResult>();

        var shortUrl = shortUrlResult.Value;
        await repository.AddAsync(shortUrl, cancellationToken);
        await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateShortUrlResult(
            shortUrl.Id,
            shortUrl.ShortCode.Value,
            shortUrl.DestinationUrl.Value,
            shortUrl.CreatedAt,
            shortUrl.ExpiresAt,
            shortUrl.IsEnabled);
    }

    /// <summary>
    /// Use custom alias if available, otherwise generate short code using id encoder
    /// </summary>
    private async Task<Result<ShortCode>> CreateShortCode(CreateShortUrlCommand command, long id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.CustomAlias))
            return ShortCode.Create(idEncoder.Encode(id));

        var codeResult = ShortCode.Create(command.CustomAlias);
        if (codeResult.IsFailure)
            return codeResult;

        if (await repository.ExistsByCodeAsync(codeResult.Value, cancellationToken))
            return Result<ShortCode>.Failure(ShortCodeErrors.AlreadyExists);

        return codeResult.Value;
    }
}
