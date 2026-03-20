using MediatR;
using UrlShortener.Application.Abstractions.Caching;

namespace UrlShortener.Application.Behaviors;

public class CachingPipelineBehavior<TRequest, TResponse>(ICacheService cache)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // chỉ apply cho query có cache
        if (request is not ICacheableQuery<TResponse> cacheable)
            return await next(cancellationToken);

        // 1. check cache
        var cached = await cache.GetAsync<TResponse>(cacheable.CacheKey, cancellationToken);
        if (cached is not null)
            return cached;

        // 2. call handler
        var response = await next(cancellationToken);

        // 3. set cache
        await cache.SetAsync(
            cacheable.CacheKey,
            response,
            cacheable.Expiration,
            cancellationToken);

        return response;
    }
}