using MediatR;
using UrlShortener.Application.Abstractions.Caching;

namespace UrlShortener.Application.Behaviors;

public class CacheInvalidationBehavior<TRequest, TResponse>(ICacheService cache)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next(cancellationToken);

        if (request is ICacheInvalidator invalidator)
        {
            foreach (var key in invalidator.Keys)
                await cache.RemoveAsync(key);
        }

        return response;
    }
}