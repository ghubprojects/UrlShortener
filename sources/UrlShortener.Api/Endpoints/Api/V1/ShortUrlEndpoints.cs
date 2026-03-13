using MediatR;
using UrlShortener.Api.Extensions;
using UrlShortener.Application.UseCases.CreateShortUrl;

namespace UrlShortener.Api.Endpoints.Api.V1;

public static class ShortUrlEndpoints
{
    public static IEndpointRouteBuilder MapShortUrlEndpointsV1(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/short-urls");

        group.MapPost("/", CreateShortUrl);

        return app;
    }

    private static async Task<IResult> CreateShortUrl(
        CreateShortUrlCommand command,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        return result.ToHttpResult();
    }
}
