using MediatR;
using UrlShortener.Api.Extensions;
using UrlShortener.Application.Features.Redirecting.UseCases.Redirect;

namespace UrlShortener.Api.Endpoints.Public;

public static class RedirectEndpoints
{
    public static IEndpointRouteBuilder MapRedirectEndpointsV1(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{code}", RedirectShortUrl);

        return app;
    }

    private static async Task<IResult> RedirectShortUrl(
        string code,
        HttpRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new RedirectQuery(
            code,
            request.HttpContext.Connection.RemoteIpAddress?.ToString(),
            request.Headers.UserAgent.ToString(),
            request.Headers.Referer.ToString());

        var result = await sender.Send(query, cancellationToken);

        return result.ToHttpResult(value => TypedResults.Redirect(value));
    }
}
