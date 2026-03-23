using UrlShortener.Shared.EventBus.Events;

namespace UrlShortener.Shared.EventBus.Abstractions;

public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : IntegrationEvent;
}