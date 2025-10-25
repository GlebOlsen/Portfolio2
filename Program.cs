using IMDB.DataService.Db;
using IMDB.DataService.Interfaces;
using IMDB.DataService.Services;
using Microsoft.EntityFrameworkCore;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

DotNetEnv.Env.Load();

var host = Environment.GetEnvironmentVariable("DB_HOST");
var database = Environment.GetEnvironmentVariable("DB_NAME");
var username = Environment.GetEnvironmentVariable("DB_USER");
var password = Environment.GetEnvironmentVariable("DB_PASS");

var connectionString = $"Host={host};Database={database};Username={username};Password={password}";

builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<ISearchService, SearchService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet(
    "/test-search",
    async (ISearchService searchService) =>
    {
        // Test parameters
        var testUserId = Guid.Parse("dd67eaed-199d-4164-9211-d6c410b923ad"); // test UUID
        var title = "matrix";
        var plot = "";
        var characters = "";
        var person = "";

        var results = await searchService.StructuredSearchAsync(
            testUserId,
            title,
            plot,
            characters,
            person,
            0,
            10
        );

        return Results.Ok(results);
    }
);

app.Run();
