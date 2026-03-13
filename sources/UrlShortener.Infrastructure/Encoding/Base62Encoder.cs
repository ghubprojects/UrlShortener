using System.Text;

namespace UrlShortener.Infrastructure.Encoding;

public static class Base62Encoder
{
    private const string Alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public static string Encode(long value)
    {
        if (value == 0) return "0";

        var result = new StringBuilder();

        while (value > 0)
        {
            result.Insert(0, Alphabet[(int)(value % 62)]);
            value /= 62;
        }

        return result.ToString();
    }

    public static long Decode(string input)
    {
        long result = 0;

        foreach (var c in input)
        {
            result = result * 62 + Alphabet.IndexOf(c);
        }

        return result;
    }
}
