using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ImdbClone.Api.Database;
using ImdbClone.Api.DTOs;
using ImdbClone.Api.DTOs.Users;
using ImdbClone.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ImdbClone.Tests;

public abstract class BaseIntegrationTest : IDisposable
{
    protected readonly HttpClient client;
    private const string BaseUrl = "http://localhost:5082"; // REMEMBER TO CHANGE THE PORT IF NEEDED!!!

    protected const string ConnectionString =
        "Host=localhost;Database=imdb;Username=postgres;Password=Pulnaskrub13%";

    protected BaseIntegrationTest()
    {
        client = new HttpClient { BaseAddress = new Uri(BaseUrl) };
    }

    public void Dispose()
    {
        client?.Dispose();
    }

    protected ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }

    protected async Task<(string Token, string Username)> RegisterAndLogin()
    {
        var username = $"testuser_{Guid.NewGuid()}";
        var password = "Password123!";
        var email = $"{username}@example.com";

        var createUserDto = new CreateUserDto
        {
            Name = "Test User",
            Username = username,
            Email = email,
            Password = password,
        };

        var registerResponse = await client.PostAsJsonAsync("/users/signup", createUserDto);
        Assert.Equal(HttpStatusCode.OK, registerResponse.StatusCode);

        var loginDto = new LoginUserDto { Username = username, Password = password };
        var loginResponse = await client.PostAsJsonAsync("/users/login", loginDto);
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        var loginResult = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
        var token = loginResult.GetProperty("token").GetString();

        return (token ?? string.Empty, username);
    }

    protected async Task<Guid> CreateTestGenre(string? genreName = null)
    {
        using var dbContext = CreateDbContext();

        genreName ??= $"TestGenre_{Guid.NewGuid()}";

        var genre = new Genre { GenreId = Guid.NewGuid(), GenreName = genreName };

        dbContext.Genres.Add(genre);
        await dbContext.SaveChangesAsync();

        return genre.GenreId;
    }

    protected async Task<Guid> CreateTestCountry(string? countryName = null)
    {
        using var dbContext = CreateDbContext();

        countryName ??= $"TestCountry_{Guid.NewGuid()}";

        var country = new Country { CountryId = Guid.NewGuid(), CountryName = countryName };

        dbContext.Countries.Add(country);
        await dbContext.SaveChangesAsync();

        return country.CountryId;
    }

    protected async Task DeleteTestGenre(Guid genreId)
    {
        using var dbContext = CreateDbContext();

        var genre = await dbContext.Genres.FindAsync(genreId);
        if (genre != null)
        {
            dbContext.Genres.Remove(genre);
            await dbContext.SaveChangesAsync();
        }
    }

    protected async Task DeleteTestCountry(Guid countryId)
    {
        using var dbContext = CreateDbContext();

        var country = await dbContext.Countries.FindAsync(countryId);
        if (country != null)
        {
            dbContext.Countries.Remove(country);
            await dbContext.SaveChangesAsync();
        }
    }
}
