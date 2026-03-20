using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Application.Abstractions.Caching;

namespace UrlShortener.Application.Behaviors;

internal class CachingPipelineBehavior
{
}
public class CachingPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ICacheService _cache;

    public CachingPipelineBehavior(ICacheService cache)
    {
        _cache = cache;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // chỉ apply cho query có cache
        if (request is not ICacheableQuery<TResponse> cacheable)
            return await next();

        // 1. check cache
        var cached = await _cache.GetAsync<TResponse>(cacheable.CacheKey);
        if (cached is not null)
            return cached;

        // 2. call handler
        var response = await next();

        // 3. set cache
        await _cache.SetAsync(
            cacheable.CacheKey,
            response,
            cacheable.Expiration);

        return response;
    }
}