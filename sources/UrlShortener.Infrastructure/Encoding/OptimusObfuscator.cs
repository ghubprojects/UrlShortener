namespace UrlShortener.Infrastructure.Encoding;

public class OptimusObfuscator(
    long prime,
    long inverse,
    long random,
    int bitLength = 31)
{
    private readonly long _max = (1L << bitLength) - 1;

    public long Encode(long value)
    {
        return ((value * prime) & _max) ^ random;
    }

    public long Decode(long value)
    {
        return ((value ^ random) * inverse) & _max;
    }
}