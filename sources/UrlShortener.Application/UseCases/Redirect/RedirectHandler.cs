using MediatR;
using UrlShortener.Domain.Aggregates.ShortUrls;
using UrlShortener.Domain.Aggregates.ShortUrls.Errors;
using UrlShortener.Shared.Results;

namespace UrlShortener.Application.UseCases.Redirect;

public sealed class RedirectHandler(IShortUrlRepository repository) : IRequestHandler<RedirectQuery, Result<string>>
{
    public async Task<Result<string>> Handle(RedirectQuery command, CancellationToken cancellationToken)
    {
        var codeResult = ShortCode.Create(command.Code);
        if (codeResult.IsFailure)
            return codeResult.ToFailure<string>();

        var shortUrl = await repository.GetByCodeAsync(codeResult.Value, cancellationToken);
        if (shortUrl is null)
            return Result<string>.Failure(ShortUrlErrors.NotFound);

        var visitResult = shortUrl.RecordVisit(DateTime.UtcNow, command.IpAddress, command.UserAgent, command.Referer);
        if (visitResult.IsFailure)
            return visitResult.ToFailure<string>();

        await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return shortUrl.DestinationUrl.Value;
    }
}
