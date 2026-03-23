using Asp.Versioning;

namespace UrlShortener.Api;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddWebApiServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddOpenApi();

        services.AddApiVersioning(
            options =>
            {
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Version")
                );
            }
        );

        return builder;
    }
}
