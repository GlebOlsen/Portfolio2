using ImdbClone.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ImdbClone.Api.Controllers;

[ApiController]
[Route("ratings")]
public class RatingsController : ControllerBase
{
    private readonly IRatingService _ratingService;

    public RatingsController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRatings([FromQuery] int? page, [FromQuery] int? pageSize)
    {
        var result = await _ratingService.GetAllRatingsAsync(page ?? 0, pageSize ?? 10);
        return Ok(result);
    }

    [HttpGet("{tconst}")]
    public async Task<IActionResult> GetRatingById(string tconst)
    {
        var result = await _ratingService.GetRatingByIdAsync(tconst);
        
        if (result == null)
            return NotFound();

        return Ok(result);
    }
}
