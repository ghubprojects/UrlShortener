using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis");

var postgres = builder.AddPostgres("postgres")
    .WithImageTag("latest")
    .WithVolume("urlshortenerdb", "/var/lib/postgresql")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithPgWeb();

var database = postgres.AddDatabase("urlshortenerdb");

var migrationService = builder.AddProject<UrlShortener_MigrationService>("migrationservice")
    .WithReference(database)
    .WaitFor(database);

var api = builder.AddProject<UrlShortener_Api>("api")
    .WithReference(database)
    .WithReference(redis)
    .WaitFor(database)
    .WaitForCompletion(migrationService);
redis.WithParentRelationship(api);

builder.Build().Run();
