using ImdbClone.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ImdbClone.Api.Controllers;

[ApiController]
[Route("episodes")]
public class EpisodesController : ControllerBase
{
    private readonly IEpisodeService _episodeService;

    public EpisodesController(IEpisodeService episodeService)
    {
        _episodeService = episodeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEpisodes([FromQuery] int? page, [FromQuery] int? pageSize)
    {
        var result = await _episodeService.GetAllEpisodesAsync(page ?? 0, pageSize ?? 10);
        return Ok(result);
    }

    [HttpGet("{tconst}")]
    public async Task<IActionResult> GetEpisodeById(string tconst)
    {
        var result = await _episodeService.GetEpisodeByIdAsync(tconst);
        
        if (result == null)
            return NotFound();

        return Ok(result);
    }
}
