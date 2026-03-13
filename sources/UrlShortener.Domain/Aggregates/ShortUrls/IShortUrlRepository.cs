using UrlShortener.Domain.SeedWork;

namespace UrlShortener.Domain.Aggregates.ShortUrls;

public interface IShortUrlRepository : IRepository<ShortUrl, long>
{
    Task<ShortUrl?> GetByIdAsync(
        long id, 
        CancellationToken cancellationToken = default);

    Task<ShortUrl?> GetByCodeAsync(
        ShortCode code,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCodeAsync(
        ShortCode code,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        ShortUrl shortUrl,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        ShortUrl shortUrl,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        ShortUrl shortUrl,
        CancellationToken cancellationToken = default);
}
