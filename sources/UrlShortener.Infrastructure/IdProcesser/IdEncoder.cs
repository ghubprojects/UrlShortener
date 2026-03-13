using UrlShortener.Application.Abstractions.IdProcesser;
using UrlShortener.Infrastructure.Encoding;

namespace UrlShortener.Infrastructure.IdProcesser;

public class IdEncoder(OptimusObfuscator optimus) : IIdEncoder
{
    public string Encode(long id)
    {
        var obfuscated = optimus.Encode(id);

        return Base62Encoder.Encode(obfuscated);
    }

    public long Decode(string value)
    {
        var decoded = Base62Encoder.Decode(value);

        return optimus.Decode(decoded);
    }
}
