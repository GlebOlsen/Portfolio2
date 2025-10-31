using ImdbClone.Api.Interfaces;
using ImdbClone.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImdbClone.Api.Controllers;

[ApiController]
[Route("episodes")]
public class EpisodesController(IEpisodeService episodeService, PaginationService paginationService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllEpisodes(
        [FromQuery] int? page,
        [FromQuery] int? pageSize
    )
    {
        var result = await episodeService.GetAllEpisodesAsync(page ?? 0, pageSize ?? 10);

        var queryParams = new Dictionary<string, string?>();

        paginationService.SetPaginationUrls(result, Request.Path, queryParams);

        return Ok(result);
    }

    [HttpGet("{tconst}")]
    public async Task<IActionResult> GetEpisodeById(string tconst)
    {
        var result = await episodeService.GetEpisodeByIdAsync(tconst);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}
