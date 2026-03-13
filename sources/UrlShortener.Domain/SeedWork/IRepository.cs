namespace UrlShortener.Domain.SeedWork;

public interface IRepository<T, TId> where T : AggregateRoot<TId> where TId : notnull
{
    IUnitOfWork UnitOfWork { get; }
}
