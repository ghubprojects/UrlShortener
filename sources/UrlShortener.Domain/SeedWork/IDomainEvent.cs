using MediatR;

namespace UrlShortener.Domain.SeedWork;

public interface IDomainEvent : INotification
{
    Guid Id { get; }
    DateTime OccurredOn { get; }
}