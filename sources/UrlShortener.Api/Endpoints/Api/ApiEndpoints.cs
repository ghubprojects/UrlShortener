using UrlShortener.Api.Endpoints.Api.V1;

namespace UrlShortener.Api.Endpoints.Api;

public static class ApiEndpoints
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        var vApi = app.NewVersionedApi("UrlShortener");

        var v1 = vApi
            .MapGroup("api/v{version:apiVersion}")
            .HasApiVersion(1, 0);

        v1.MapShortUrlEndpointsV1();

        return app;
    }
}