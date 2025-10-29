using ImdbClone.Api.Interfaces;
using ImdbClone.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImdbClone.Api.Controllers;

[ApiController]
[Route("title")]
public class TitleController(ITitleService titleService, PaginationService paginationService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllTitlesAsync(
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10
    )
    {
        var result = await titleService.GetAllTitlesAsync(page, pageSize);

        var queryParams = new Dictionary<string, string?>();

        paginationService.SetPaginationUrls(result, Request.Path, queryParams);

        return Ok(result);
    }

    [HttpGet("{tconst}")]
    public async Task<IActionResult> GetTitleById(string tconst)
    {
        var result = await titleService.GetTitleByIdAsync(tconst);

        if (result == null)
        {
            return NotFound(new { message = $"Title '{tconst}' not found" });
        }

        return Ok(result);
    }

    [HttpGet("genre/{genreName}")]
    public async Task<IActionResult> GetTitlesByGenre(
        string genreName,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10
    )
    {
        var result = await titleService.GetTitlesByGenre(genreName, page, pageSize);

        var queryParams = new Dictionary<string, string?>();

        paginationService.SetPaginationUrls(result, Request.Path, queryParams);

        return Ok(result);
    }

    [HttpGet("person/{nconst}")]
    public async Task<IActionResult> GetTitlesByPersonAsync(
        string nconst,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10
    )
    {
        var result = await titleService.GetTitlesByPersonAsync(nconst, page, pageSize);

        var queryParams = new Dictionary<string, string?>();

        paginationService.SetPaginationUrls(result, Request.Path, queryParams);

        return Ok(result);
    }

    [HttpGet("type/{titleType}")]
    public async Task<IActionResult> GetTitlesByTypeAsync(
        string titleType,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10
    )
    {
        var result = await titleService.GetTitlesByTypeAsync(titleType, page, pageSize);

        var queryParams = new Dictionary<string, string?>();

        paginationService.SetPaginationUrls(result, Request.Path, queryParams);

        return Ok(result);
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetTotalTitlesCountAsync()
    {
        var count = await titleService.GetTotalTitlesCountAsync();
        return Ok(new { total = count });
    }
}
