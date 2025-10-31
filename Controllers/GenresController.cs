using ImdbClone.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ImdbClone.Api.Controllers;

[ApiController]
[Route("genres")]
public class GenresController : ControllerBase
{
    private readonly IGenreService _genreService;

    public GenresController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGenres([FromQuery] int? page, [FromQuery] int? pageSize)
    {
        var result = await _genreService.GetAllGenresAsync(page ?? 0, pageSize ?? 10);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGenreById(Guid id)
    {
        var result = await _genreService.GetGenreByIdAsync(id);
        
        if (result == null)
            return NotFound();

        return Ok(result);
    }
}
