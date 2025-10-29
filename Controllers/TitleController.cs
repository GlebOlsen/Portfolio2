using ImdbClone.Api.Interfaces;
using ImdbClone.Api.Services;
using Microsoft.AspNetCore.Mvc;

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

        var queryParams = new Dictionary<string, string?> { { "pageSize", pageSize.ToString() } };

        paginationService.SetPaginationUrls(result, Request.Path, queryParams);

        return Ok(result);
    }

    [HttpGet("{tconst}")]
    public async Task<IActionResult> GetTitleById(string tconst)
    {
        var title = await titleService.GetTitleByIdAsync(tconst);

        if (title == null)
        {
            return NotFound(new { message = $"Title '{tconst}' not found" });
        }

        return Ok(title);
    }

    [HttpGet("genre/{genreName}")]
    public async Task<IActionResult> GetTitlesByGenre(
        string genreName,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10
    )
    {
        var result = await titleService.GetTitlesByGenre(genreName, page, pageSize);
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
        return Ok(result);
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetTotalTitlesCountAsync()
    {
        var count = await titleService.GetTotalTitlesCountAsync();
        return Ok(new { total = count });
    }
}
