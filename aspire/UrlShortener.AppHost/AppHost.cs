using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// Add PostgreSQL
var postgres = builder.AddPostgres("postgres")
    .WithImageTag("latest")
    .WithVolume("urlshortenerdb", "/var/lib/postgresql")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithPgWeb();

var database = postgres.AddDatabase("urlshortenerdb");

var migrationService = builder.AddProject<UrlShortener_MigrationService>("migrationservice")
    .WithReference(database)
    .WaitFor(database);

builder.AddProject<UrlShortener_Api>("api")
    .WithReference(database)
    .WaitFor(database)
    .WaitForCompletion(migrationService);

builder.Build().Run();
