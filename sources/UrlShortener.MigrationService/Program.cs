using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using UrlShortener.Infrastructure.Persistence.DataContext;
using UrlShortener.MigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.AddNpgsqlDbContext<AppDbContext>("urlshortenerdb", configureDbContextOptions: optionBuilder =>
{
    optionBuilder
        .UseNpgsql(builder => builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
        .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning)) // Ignore the warning about pending model changes
        .EnableDetailedErrors();
});

var host = builder.Build();
host.Run();
