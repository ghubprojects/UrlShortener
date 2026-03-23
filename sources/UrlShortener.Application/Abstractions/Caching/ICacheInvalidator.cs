namespace UrlShortener.Application.Abstractions.Caching;

public interface ICacheInvalidator
{
    IEnumerable<string> Keys { get; }
}