namespace UrlShortener.Application.Abstractions.IdProcesser;

public interface IIdEncoder
{
    string Encode(long id);
    long Decode(string value);
}
