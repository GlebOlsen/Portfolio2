using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Xunit;

namespace ImdbClone.Tests;

public class ApiEndpointsTests
{
    private readonly HttpClient client;
    private const string BaseUrl = "http://localhost:5082"; // REMEMBER TO CHANGE THE PORT IF NEEDED!!!


    public ApiEndpointsTests()
    {
        client = new HttpClient { BaseAddress = new Uri(BaseUrl) };
    }

    [Fact]
    public async Task GetAllCountries_ReturnsSuccess()
    {
        var response = await client.GetAsync("/countries");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetCountryById_ReturnsSuccess()
    {
        var response = await client.GetAsync("/countries/d738292e-82fc-4e4e-8a41-1bba68572be5");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetAllGenres_ReturnsSuccess()
    {
        var response = await client.GetAsync("/genres");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetGenreById_ReturnsSuccess()
    {
        var response = await client.GetAsync("/genres/3f34f1a0-8fc4-4b0e-b1b1-91730788dc0d");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetAllEpisodes_ReturnsSuccess()
    {
        var response = await client.GetAsync("/episodes?pageSize=1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetEpisodeById_ReturnsSuccess()
    {
        var response = await client.GetAsync("/episodes/tt0098286");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetAllRatings_ReturnsSuccess()
    {
        var response = await client.GetAsync("/ratings?pageSize=1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetRatingById_ReturnsSuccess()
    {
        var response = await client.GetAsync("/ratings/tt0052520");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetAllTitleAliases_ReturnsSuccess()
    {
        var response = await client.GetAsync("/title-aliases?pageSize=1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetTitleAliasById_ReturnsSuccess()
    {
        var response = await client.GetAsync("/title-aliases/tt0052520/1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetAllTitles_ReturnsSuccess()
    {
        var response = await client.GetAsync("/titles?pageSize=1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetTitleById_ReturnsSuccess()
    {
        var response = await client.GetAsync("/titles/tt1641917");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetTitleCount_ReturnsSuccess()
    {
        var response = await client.GetAsync("/titles/count");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetCountriesByTitle_ReturnsSuccess()
    {
        var response = await client.GetAsync("/titles/tt1641917/countries");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetGenresByTitle_ReturnsSuccess()
    {
        var response = await client.GetAsync("/titles/tt22187886/genres");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetTitlesByGenre_ReturnsSuccess()
    {
        var response = await client.GetAsync("/titles/genre/Action?pageSize=1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetTitlesByPerson_ReturnsSuccess()
    {
        var response = await client.GetAsync("/titles/person/nm0000001?pageSize=1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetTitlesByType_ReturnsSuccess()
    {
        var response = await client.GetAsync("/titles/type/movie?pageSize=1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task StructuredSearch_ReturnsSuccess()
    {
        var response = await client.GetAsync("/search/structured-search?title=Matrix");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task StringSearch_ReturnsSuccess()
    {
        var response = await client.GetAsync("/search/string-search?query=action");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task FindNames_ReturnsSuccess()
    {
        var response = await client.GetAsync("/search/find-names?query=Tom");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task FindNamesByProfession_ReturnsSuccess()
    {
        var response = await client.GetAsync("/search/find-names-by-profession?name=Tom&profession=actor");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task SearchTitlesExact_ReturnsSuccess()
    {
        var response = await client.GetAsync("/search/search-titles-exact?words=Matrix");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task SearchTitlesBestMatch_ReturnsSuccess()
    {
        var response = await client.GetAsync("/search/search-titles-best-match?words=Matrix");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task SearchWordsToWords_ReturnsSuccess()
    {
        var response = await client.GetAsync("/search/search-words-to-words?words=action,hero");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
