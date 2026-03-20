using Microsoft.Extensions.DependencyInjection;

namespace UrlShortener.Shared.EventBus.Abstractions;

public interface IEventBusBuilder
{
    public IServiceCollection Services { get; }
}
