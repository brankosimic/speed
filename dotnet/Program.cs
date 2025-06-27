using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using System.Data;
using Dapper;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxConcurrentConnections = 10000;
    serverOptions.Limits.MaxConcurrentUpgradedConnections = 10000;
    serverOptions.Limits.MaxRequestBodySize = 52428800; // 50MB
    serverOptions.ConfigureEndpointDefaults(listenOptions => {
    listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
});
});

ThreadPool.SetMinThreads(workerThreads: 100, completionPortThreads: 100);
ThreadPool.SetMaxThreads(workerThreads: 32767, completionPortThreads: 32767);


builder.Services.AddOpenApi();
builder.Services.AddSingleton<AppDbContext>();
builder.Services.AddScoped<IFilmRepository, FilmRepository>();

var app = builder.Build();

Console.WriteLine($"Kestrel/ASP.NET Core will use {Environment.ProcessorCount} CPU cores.");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var jsonOptions = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    TypeInfoResolver = AppJsonContext.Default
};

app.MapGet("/films", async (IFilmRepository repo) =>
{
    var films = (await repo.GetAllAsync()).ToArray();
    return Results.Text(JsonSerializer.Serialize(films, AppJsonContext.Default.FilmArray), "application/json");
});

app.MapGet("/films/{id}", async (int id, IFilmRepository repo) =>
{
    var film = await repo.GetByIdAsync(id);
    if (film is null) return Results.NotFound();
    return Results.Text(JsonSerializer.Serialize(film, AppJsonContext.Default.Film), "application/json");
});

app.MapPost("/films", async (Film film, IFilmRepository repo) =>
{
    var created = await repo.AddAsync(film);
    return Results.Text(JsonSerializer.Serialize(created, AppJsonContext.Default.Film), "application/json", statusCode: 201);
});

app.MapPut("/films/{id}", async (int id, Film film, IFilmRepository repo) =>
{
    if (id != film.FilmId) return Results.BadRequest();
    var updated = await repo.UpdateAsync(film);
    if (updated is null) return Results.NotFound();
    return Results.Text(JsonSerializer.Serialize(updated, AppJsonContext.Default.Film), "application/json");
});

app.MapDelete("/films/{id}", async (int id, IFilmRepository repo) =>
{
    var deleted = await repo.DeleteAsync(id);
    return deleted ? Results.NoContent() : Results.NotFound();
});

app.Run();