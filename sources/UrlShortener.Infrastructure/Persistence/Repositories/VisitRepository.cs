using UrlShortener.Domain.Entities.Visits;
using UrlShortener.Infrastructure.Persistence.DataContext;

namespace UrlShortener.Infrastructure.Persistence.Repositories;

public sealed class VisitRepository(AppDbContext context) : IVisitRepository
{
    public async Task AddAsync(Visit visit, CancellationToken cancellationToken = default)
    {
        await context.Visits.AddAsync(visit, cancellationToken);
    }

    public Task<long> CountByShortUrlIdAsync(Guid shortUrlId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<long> CountByShortUrlIdSinceAsync(Guid shortUrlId, DateTime since, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Visit>> GetByShortUrlIdAsync(Guid shortUrlId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
