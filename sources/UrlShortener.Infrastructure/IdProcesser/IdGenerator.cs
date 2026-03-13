using IdGen;
using UrlShortener.Application.Abstractions.IdProcesser;

namespace UrlShortener.Infrastructure.IdProcesser;

public class IdGenerator : IIdGenerator
{
    private readonly IdGen.IdGenerator _generator;

    public IdGenerator()
    {
        var structure = new IdStructure(41, 10, 12);
        var epoch = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var options = new IdGeneratorOptions(structure, new DefaultTimeSource(epoch));
        
        _generator = new IdGen.IdGenerator(1, options);
    }

    public long Generate()
    {
        return _generator.CreateId();
    }
}
