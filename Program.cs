using Microsoft.EntityFrameworkCore;
using IMDB.DataService.Db;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

DotNetEnv.Env.Load();

var host = Environment.GetEnvironmentVariable("DB_HOST");
var database = Environment.GetEnvironmentVariable("DB_NAME");
var username = Environment.GetEnvironmentVariable("DB_USER");
var password = Environment.GetEnvironmentVariable("DB_PASS");

var connectionString = $"Host={host};Database={database};Username={username};Password={password}";

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(connectionString));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapGet("/testdb", async (DatabaseContext db) =>
{
    var count = await db.Titles.CountAsync();
    return $"Titles in DB: {count}";
});

app.Run();

