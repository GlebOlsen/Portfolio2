using ImdbClone.Api.Interfaces;
using ImdbClone.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImdbClone.Api.Controllers;

[ApiController]
[Route("title-aliases")]
public class TitleAliasesController(
    ITitleAliasService titleAliasService,
    PaginationService paginationService
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllTitleAliases(
        [FromQuery] int? page,
        [FromQuery] int? pageSize
    )
    {
        var result = await titleAliasService.GetAllTitleAliasesAsync(page ?? 0, pageSize ?? 10);

        var queryParams = new Dictionary<string, string?>();
        paginationService.SetPaginationUrls(result, Request.Path, queryParams);

        return Ok(result);
    }

    [HttpGet("{tconst}/{ordering}")]
    public async Task<IActionResult> GetTitleAliasById(string tconst, int ordering)
    {
        var result = await titleAliasService.GetTitleAliasByIdAsync(tconst, ordering);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}
