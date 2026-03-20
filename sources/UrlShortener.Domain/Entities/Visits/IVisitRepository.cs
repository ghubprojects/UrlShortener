namespace UrlShortener.Domain.Entities.Visits;

public interface IVisitRepository
{
    Task AddAsync(
        Visit visit,
        CancellationToken cancellationToken = default);

    Task<long> CountByShortUrlIdAsync(
        Guid shortUrlId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Visit>> GetByShortUrlIdAsync(
        Guid shortUrlId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<long> CountByShortUrlIdSinceAsync(
        Guid shortUrlId,
        DateTime since,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
