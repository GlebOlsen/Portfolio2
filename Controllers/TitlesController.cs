using ImdbClone.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ImdbClone.Api.Controllers;

[ApiController]
[Route("titles")]
public class TitlesController : ControllerBase
{
    private readonly ITitleService _titleService;

    public TitlesController(ITitleService titleService)
    {
        _titleService = titleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTitles([FromQuery] int? page, [FromQuery] int? pageSize)
    {
        var result = await _titleService.GetAllTitlesAsync(page ?? 0, pageSize ?? 10);
        return Ok(result);
    }

    [HttpGet("{tconst}")]
    public async Task<IActionResult> GetTitleById(string tconst)
    {
        var result = await _titleService.GetTitleByIdAsync(tconst);
        
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("{tconst}/countries")]
    public async Task<IActionResult> GetCountriesByTitle(string tconst)
    {
        var result = await _titleService.GetCountriesByTitleAsync(tconst);
        return Ok(result);
    }

    [HttpGet("{tconst}/genres")]
    public async Task<IActionResult> GetGenresByTitle(string tconst)
    {
        var result = await _titleService.GetGenresByTitleAsync(tconst);
        return Ok(result);
    }
}
