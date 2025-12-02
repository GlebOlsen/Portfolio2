using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ImdbClone.Api.DTOs;
using ImdbClone.Api.DTOs.Users;
using Xunit;

namespace ImdbClone.Tests;

public class UserEndpointsTests
{
    private readonly HttpClient client;
    private const string BaseUrl = "http://localhost:5082";

    public UserEndpointsTests()
    {
        client = new HttpClient { BaseAddress = new Uri(BaseUrl) };
    }

    private async Task<(string Token, string Username)> RegisterAndLogin()
    {
        var username = $"testuser_{Guid.NewGuid()}";
        var password = "Password123!";
        var email = $"{username}@example.com";

        var createUserDto = new CreateUserDto
        {
            Name = "Test User",
            Username = username,
            Email = email,
            Password = password
        };

        var registerResponse = await client.PostAsJsonAsync("/users", createUserDto);
        Assert.Equal(HttpStatusCode.OK, registerResponse.StatusCode);

        var loginDto = new LoginUserDto
        {
            Username = username,
            Password = password
        };

        var loginResponse = await client.PostAsJsonAsync("/users/login", loginDto);
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        var loginResult = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
        var token = loginResult.GetProperty("token").GetString();

        return (token ?? string.Empty, username);
    }

    [Fact]
    public async Task Register_ReturnsSuccess()
    {
        var username = $"testuser_{Guid.NewGuid()}";
        var password = "Password123!";
        var email = $"{username}@example.com";

        var createUserDto = new CreateUserDto
        {
            Name = "Test User",
            Username = username,
            Email = email,
            Password = password
        };

        var response = await client.PostAsJsonAsync("/users", createUserDto);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Login_ReturnsSuccess()
    {
        var (token, _) = await RegisterAndLogin();
        Assert.NotNull(token);
    }

    [Fact]
    public async Task DeleteUser_ReturnsSuccess()
    {
        var (token, _) = await RegisterAndLogin();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.DeleteAsync("/users");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task BookmarkTitle_ReturnsSuccess()
    {
        var (token, _) = await RegisterAndLogin();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var tconst = "tt0098286";
        var bookmarkDto = new CreateBookmarkTitleDto { Tconst = tconst };
        var response = await client.PostAsJsonAsync("/users/bookmark-title", bookmarkDto);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var getResponse = await client.GetAsync("/users/bookmark-title");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        var deleteDto = new DeleteBookmarkTitleDto { Tconst = tconst };
        var request = new HttpRequestMessage(HttpMethod.Delete, "/users/bookmark-title")
        {
            Content = JsonContent.Create(deleteDto)
        };
        var deleteResponse = await client.SendAsync(request);
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task BookmarkPerson_ReturnsSuccess()
    {
        var (token, _) = await RegisterAndLogin();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var nconst = "nm0000001";
        var createDto = new CreateBookmarkPersonDto { Nconst = nconst };
        var createResponse = await client.PostAsJsonAsync("/users/bookmark-person", createDto);
        Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);
        var getResponse = await client.GetAsync("/users/bookmark-person");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        var deleteDto = new DeleteBookmarkPersonDto { Nconst = nconst };
        var request = new HttpRequestMessage(HttpMethod.Delete, "/users/bookmark-person")
        {
            Content = JsonContent.Create(deleteDto)
        };
        var deleteResponse = await client.SendAsync(request);
        Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task RateTitle_ReturnsSuccess()
    {
        var (token, _) = await RegisterAndLogin();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var tconst = "tt0098286";
        var rating = 8;
        var createDto = new CreateTitleRatingDto { Tconst = tconst, Rating = rating };
        var createResponse = await client.PostAsJsonAsync("/users/rate-title", createDto);
        Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);
        var getResponse = await client.GetAsync("/users/rate-title");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        var deleteDto = new DeleteTitleRatingDto { Tconst = tconst };
        var request = new HttpRequestMessage(HttpMethod.Delete, "/users/rate-title")
        {
            Content = JsonContent.Create(deleteDto)
        };
        var deleteResponse = await client.SendAsync(request);
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task SearchHistory_ReturnsSuccess()
    {
        var (token, _) = await RegisterAndLogin();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var searchResponse = await client.GetAsync("/search/structured-search?title=Matrix");
        Assert.Equal(HttpStatusCode.OK, searchResponse.StatusCode);
        var getResponse = await client.GetAsync("/users/search-history");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        var deleteResponse = await client.DeleteAsync("/users/search-history");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }
}
