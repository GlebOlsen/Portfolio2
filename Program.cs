using System.Text;
using IMDB.DataService.Services;
using ImdbClone.Api.Database;
using ImdbClone.Api.Interfaces;
using ImdbClone.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddOpenApi();

builder.Services.AddControllers();

DotNetEnv.Env.Load();

var host = Environment.GetEnvironmentVariable("DB_HOST");
var database = Environment.GetEnvironmentVariable("DB_NAME");
var username = Environment.GetEnvironmentVariable("DB_USER");
var password = Environment.GetEnvironmentVariable("DB_PASS");

var connectionString = $"Host={host};Database={database};Username={username};Password={password}";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention()
);

builder.Services.AddSingleton<Hashing>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ITitleService, TitleService>();
builder.Services.AddTransient<ICountryService, CountryService>();
builder.Services.AddTransient<IGenreService, GenreService>();
builder.Services.AddTransient<IEpisodeService, EpisodeService>();
builder.Services.AddTransient<IRatingService, RatingService>();
builder.Services.AddTransient<ITitleAliasService, TitleAliasService>();

var secret = Environment.GetEnvironmentVariable("JWT_SECRET");

builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero,
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
