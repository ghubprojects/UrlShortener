using UrlShortener.Domain.Aggregates.ShortUrls.Errors;
using UrlShortener.Domain.SeedWork;
using UrlShortener.Shared.Results;

namespace UrlShortener.Domain.Aggregates.ShortUrls;

public sealed class Url : ValueObject
{
    public string Value { get; }

    private Url(string value)
    {
        Value = value;
    }

    public static Result<Url> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Url>(UrlErrors.Empty);

        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri))
            return Result.Failure<Url>(UrlErrors.InvalidFormat);

        if (uri.Scheme != Uri.UriSchemeHttp &&
            uri.Scheme != Uri.UriSchemeHttps)
            return Result.Failure<Url>(UrlErrors.InvalidScheme);

        return new Url(uri.ToString());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value.ToLowerInvariant();
    }

    public override string ToString() => Value;

    public static implicit operator string(Url url)
        => url.Value;
}
