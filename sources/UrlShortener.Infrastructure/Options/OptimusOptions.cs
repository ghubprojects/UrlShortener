namespace UrlShortener.Infrastructure.Options;

public sealed class OptimusOptions
{
    public const string SectionName = "Optimus";

    public long Prime { get; init; }

    public long Inverse { get; init; }

    public long Random { get; init; }

    public int BitLength { get; init; } = 31;
}
