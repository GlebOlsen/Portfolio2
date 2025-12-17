using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ImdbClone.Api.DTOs;
using ImdbClone.Api.DTOs.Users;
using Xunit;
using Xunit;

namespace ImdbClone.Tests;

public class ApiEndpointsTests : BaseIntegrationTest
{
    [Fact]
    public async Task GetAllCountries_ReturnsSuccess()
    {
        var response = await client.GetAsync("/countries");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetCountryById_ReturnsSuccess()
    {
        var countryId = await CreateTestCountry();

        try
        {
            var response = await client.GetAsync($"/countries/{countryId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        finally
        {
            await DeleteTestCountry(countryId);
        }
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
        var genreId = await CreateTestGenre();

        try
        {
            var response = await client.GetAsync($"/genres/{genreId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        finally
        {
            await DeleteTestGenre(genreId);
        }
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
        var response = await client.GetAsync(
            "/search/find-names-by-profession?name=Tom&profession=actor"
        );
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

    [Fact]
    public async Task GetSimilarTitles_ReturnsSuccess()
    {
        var response = await client.GetAsync("/titles/tt0052520/similar");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
