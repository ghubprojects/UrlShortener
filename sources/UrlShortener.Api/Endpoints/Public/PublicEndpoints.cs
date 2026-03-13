namespace UrlShortener.Api.Endpoints.Public;

public static class PublicEndpoints
{
    public static IEndpointRouteBuilder MapPublicEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapRedirectEndpointsV1();

        return app;
    }
}