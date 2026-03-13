using UrlShortener.Api;
using UrlShortener.Api.Endpoints.Api;
using UrlShortener.Api.Endpoints.Public;
using UrlShortener.Application;
using UrlShortener.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container
builder.Services.AddApplicationServices();
builder.AddInfrastructureServices();
builder.Services.AddWebApiServices();

var app = builder.Build();

app.MapDefaultEndpoints();

// Map public and API endpoints
app.MapPublicEndpoints();
app.MapApiEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();
