using ImdbClone.Api.Interfaces;
using ImdbClone.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImdbClone.Api.Controllers;

[ApiController]
[Route("ratings")]
public class RatingsController(IRatingService ratingService, PaginationService paginationService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllRatings([FromQuery] int? page, [FromQuery] int? pageSize)
    {
        var result = await ratingService.GetAllRatingsAsync(page ?? 0, pageSize ?? 10);

        var queryParams = new Dictionary<string, string?>();
        paginationService.SetPaginationUrls(result, Request.Path, queryParams);

        return Ok(result);
    }

    [HttpGet("{tconst}")]
    public async Task<IActionResult> GetRatingById(string tconst)
    {
        var result = await ratingService.GetRatingByIdAsync(tconst);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}
