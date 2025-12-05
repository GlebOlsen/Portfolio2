using System.Net;
using System.Net.Http.Json;
using ImdbClone.Api.DTOs;
using ImdbClone.Api.DTOs.Users;
using Xunit;

namespace ImdbClone.Tests;

public class UserEndpointsTests : BaseIntegrationTest
{
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
            Password = password,
        };

        var response = await client.PostAsJsonAsync("/users/signup", createUserDto);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await DeleteCurrentUser();
    }

    [Fact]
    public async Task Login_ReturnsSuccess()
    {
        var username = await RegisterAndLogin();
        Assert.NotNull(username);
        Assert.NotEmpty(username);

        await DeleteCurrentUser();
    }

    [Fact]
    public async Task DeleteUser_ReturnsSuccess()
    {
        await RegisterAndLogin();

        var response = await client.DeleteAsync("/users");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task BookmarkTitle_ReturnsSuccess()
    {
        await RegisterAndLogin();

        var tconst = "tt0098286";
        var bookmarkDto = new CreateBookmarkTitleDto { Tconst = tconst };
        var response = await client.PostAsJsonAsync("/users/bookmark-title", bookmarkDto);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var getResponse = await client.GetAsync("/users/bookmark-title");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var deleteDto = new DeleteBookmarkTitleDto { Tconst = tconst };
        var request = new HttpRequestMessage(HttpMethod.Delete, "/users/bookmark-title")
        {
            Content = JsonContent.Create(deleteDto),
        };
        var deleteResponse = await client.SendAsync(request);
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        await DeleteCurrentUser();
    }

    [Fact]
    public async Task BookmarkPerson_ReturnsSuccess()
    {
        await RegisterAndLogin();

        var nconst = "nm0000001";
        var createDto = new CreateBookmarkPersonDto { Nconst = nconst };
        var createResponse = await client.PostAsJsonAsync("/users/bookmark-person", createDto);
        Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);

        var getResponse = await client.GetAsync("/users/bookmark-person");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var deleteDto = new DeleteBookmarkPersonDto { Nconst = nconst };
        var request = new HttpRequestMessage(HttpMethod.Delete, "/users/bookmark-person")
        {
            Content = JsonContent.Create(deleteDto),
        };
        var deleteResponse = await client.SendAsync(request);
        Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

        await DeleteCurrentUser();
    }

    [Fact]
    public async Task RateTitle_ReturnsSuccess()
    {
        await RegisterAndLogin();

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
            Content = JsonContent.Create(deleteDto),
        };
        var deleteResponse = await client.SendAsync(request);
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        await DeleteCurrentUser();
    }

    [Fact]
    public async Task SearchHistory_ReturnsSuccess()
    {
        await RegisterAndLogin();

        var searchResponse = await client.GetAsync("/search/structured-search?title=Matrix");
        Assert.Equal(HttpStatusCode.OK, searchResponse.StatusCode);

        var getResponse = await client.GetAsync("/users/search-history");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var deleteResponse = await client.DeleteAsync("/users/search-history");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        await DeleteCurrentUser();
    }
}
