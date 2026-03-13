using System.Text.RegularExpressions;
using UrlShortener.Domain.Aggregates.ShortUrls.Errors;
using UrlShortener.Domain.SeedWork;
using UrlShortener.Shared.Results;

namespace UrlShortener.Domain.Aggregates.ShortUrls;

public sealed class ShortCode : ValueObject
{
    public string Value { get; }

    private static readonly Regex Pattern = new("^[a-zA-Z0-9\\-]{3,16}$", RegexOptions.Compiled);

    private ShortCode(string value)
    {
        Value = value;
    }

    public static Result<ShortCode> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<ShortCode>(ShortCodeErrors.Required);

        value = value.Trim();

        if (!Pattern.IsMatch(value))
            return Result.Failure<ShortCode>(ShortCodeErrors.InvalidFormat);

        return new ShortCode(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(ShortCode code) => code.Value;
}
