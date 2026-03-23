using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace UrlShortener.Shared.EventBus.Abstractions;

public class EventBusSubscriptionInfo
{
    public Dictionary<string, Type> EventTypes { get; } = [];

    public JsonSerializerOptions JsonSerializerOptions { get; } = new(DefaultSerializerOptions);

    internal static readonly JsonSerializerOptions DefaultSerializerOptions = new()
    {
        TypeInfoResolver = JsonSerializer.IsReflectionEnabledByDefault 
            ? new DefaultJsonTypeInfoResolver() 
            : JsonTypeInfoResolver.Combine()
    };
}
