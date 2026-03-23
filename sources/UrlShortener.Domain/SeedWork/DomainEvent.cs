namespace UrlShortener.Domain.SeedWork;

public abstract record DomainEvent : IDomainEvent
{
    public Guid Id { get; } = Guid.CreateVersion7();
    public DateTime CreationDate { get; } = DateTime.UtcNow;
}
