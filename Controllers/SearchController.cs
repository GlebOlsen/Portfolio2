using ImdbClone.Api.Interfaces;
using ImdbClone.Api.Services;
using ImdbClone.Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImdbClone.Api.Controllers;

[ApiController]
[Route("search")]
public class SearchController(ISearchService searchService, PaginationService paginationService)
    : ControllerBase
{
    [Authorize]
    [HttpGet("structured-search")]
    public async Task<IActionResult> StructuredSearch(
        string? title,
        string? plot,
        string? characters,
        string? person,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10
    )
    {
        var userId = User.GetUserId();

        if (!userId.HasValue)
        {
            return Unauthorized("User authentication required");
        }

        var result = await searchService.StructuredSearchAsync(
            userId.Value,
            title,
            plot,
            characters,
            person,
            page,
            pageSize
        );

        var queryParams = new Dictionary<string, string?>();

        if (!string.IsNullOrEmpty(title))
            queryParams["title"] = title;
        if (!string.IsNullOrEmpty(plot))
            queryParams["plot"] = plot;
        if (!string.IsNullOrEmpty(characters))
            queryParams["characters"] = characters;
        if (!string.IsNullOrEmpty(person))
            queryParams["person"] = person;

        paginationService.SetPaginationUrls(result, Request.Path, queryParams);

        return Ok(result);
    }

    [HttpGet("string-search")]
    public async Task<IActionResult> StringSearch(
        string query,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10
    )
    {
        var userId = User.GetUserId();

        var result = await searchService.StringSearch(query, page, pageSize);

        var queryParams = new Dictionary<string, string?> { { "query", query } };

        paginationService.SetPaginationUrls(result, Request.Path, queryParams);

        return Ok(result);
    }

    [Authorize]
    [HttpGet("find-names")]
    public async Task<IActionResult> FindNames(
        string query,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10
    )
    {
        var userId = User.GetUserId();

        var result = await searchService.FindNames(userId, query, page, pageSize);

        var queryParams = new Dictionary<string, string?> { { "query", query } };

        paginationService.SetPaginationUrls(result, Request.Path, queryParams);

        return Ok(result);
    }

    [HttpGet("find-names-by-profession")]
    public async Task<IActionResult> FindNamesByProfession(
        string name,
        string? profession = null,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10
    )
    {
        var userId = User.GetUserId();

        var result = await searchService.FindNamesByProfession(
            userId,
            name,
            profession,
            page,
            pageSize
        );

        var queryParams = new Dictionary<string, string?>();

        if (!string.IsNullOrEmpty(profession))
            queryParams["profession"] = profession;

        paginationService.SetPaginationUrls(result, Request.Path, queryParams);

        return Ok(result);
    }

    [HttpGet("search-titles-exact")]
    public async Task<IActionResult> SearchTitlesExact(
        [FromQuery] string words,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10
    )
    {
        if (string.IsNullOrWhiteSpace(words))
            return BadRequest(new { message = "Words parameter is required" });

        var wordsList = words
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(w => w.Trim())
            .ToList();

        var userId = User.GetUserId();

        var result = await searchService.SearchTitlesExact(wordsList, page, pageSize);

        var queryParams = new Dictionary<string, string?> { { "words", words } };
        paginationService.SetPaginationUrls(result, Request.Path, queryParams);

        return Ok(result);
    }

    [HttpGet("search-titles-best-match")]
    public async Task<IActionResult> SearchTitlesBestMatch(
        [FromQuery] string words,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10
    )
    {
        if (string.IsNullOrWhiteSpace(words))
            return BadRequest(new { message = "Words parameter is required" });

        var wordsList = words
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(w => w.Trim())
            .ToList();

        var userId = User.GetUserId();

        var result = await searchService.SearchTitlesBestMatch(wordsList, page, pageSize);

        var queryParams = new Dictionary<string, string?> { { "words", words } };
        paginationService.SetPaginationUrls(result, Request.Path, queryParams);

        return Ok(result);
    }

    [HttpGet("search-words-to-words")]
    public async Task<IActionResult> SearchWordsToWords(
        [FromQuery] string words,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10
    )
    {
        if (string.IsNullOrWhiteSpace(words))
            return BadRequest(new { message = "Words parameter is required" });

        var wordsList = words
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(w => w.Trim())
            .ToList();

        var userId = User.GetUserId();

        var result = await searchService.SearchWordsToWords(wordsList, page, pageSize);

        var queryParams = new Dictionary<string, string?> { { "words", words } };
        paginationService.SetPaginationUrls(result, Request.Path, queryParams);

        return Ok(result);
    }
}
