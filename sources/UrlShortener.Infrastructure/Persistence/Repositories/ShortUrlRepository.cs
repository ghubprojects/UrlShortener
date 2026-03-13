using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.Aggregates.ShortUrls;
using UrlShortener.Domain.SeedWork;
using UrlShortener.Infrastructure.Persistence.DataContext;

namespace UrlShortener.Infrastructure.Persistence.Repositories;

public sealed class ShortUrlRepository(AppDbContext context) : IShortUrlRepository
{
    public IUnitOfWork UnitOfWork => context;

    public async Task<ShortUrl?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        return await context.ShortUrls
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<ShortUrl?> GetByCodeAsync(
        ShortCode code,
        CancellationToken cancellationToken = default)
    {
        return await context.ShortUrls
            .FirstOrDefaultAsync(x => x.ShortCode == code, cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(
        ShortCode code,
        CancellationToken cancellationToken = default)
    {
        return await context.ShortUrls
            .AnyAsync(x => x.ShortCode == code, cancellationToken);
    }

    public async Task AddAsync(
        ShortUrl shortUrl,
        CancellationToken cancellationToken = default)
    {
        await context.ShortUrls.AddAsync(shortUrl, cancellationToken);
    }

    public Task UpdateAsync(
        ShortUrl shortUrl,
        CancellationToken cancellationToken = default)
    {
        context.ShortUrls.Update(shortUrl);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(
        ShortUrl shortUrl,
        CancellationToken cancellationToken = default)
    {
        context.ShortUrls.Remove(shortUrl);
        return Task.CompletedTask;
    }
}
