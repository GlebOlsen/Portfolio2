using ImdbClone.Api.Interfaces;
using ImdbClone.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImdbClone.Api.Controllers;

[ApiController]
[Route("structured-search")]
public class SearchController(ISearchService searchService, PaginationService paginationService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> StructuredSearch(
        Guid userId,
        string? title,
        string? plot,
        string? characters,
        string? person,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10
    )
    {
        var result = await searchService.StructuredSearchAsync(
            userId,
            title,
            plot,
            characters,
            person,
            page,
            pageSize
        );

        var queryParams = new Dictionary<string, string?>
        {
            { "userId", userId.ToString() },
            { "pageSize", pageSize.ToString() },
        };

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
}
