using ImdbClone.Api.Interfaces;
using ImdbClone.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImdbClone.Api.Controllers;

[ApiController]
[Route("genres")]
public class GenresController(IGenreService genreService, PaginationService paginationService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllGenres([FromQuery] int? page, [FromQuery] int? pageSize)
    {
        var result = await genreService.GetAllGenresAsync(page ?? 0, pageSize ?? 10);

        var queryParams = new Dictionary<string, string?>();
        paginationService.SetPaginationUrls(result, Request.Path, queryParams);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGenreById(Guid id)
    {
        var result = await genreService.GetGenreByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}
