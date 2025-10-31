using ImdbClone.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ImdbClone.Api.Controllers;

[ApiController]
[Route("title-aliases")]
public class TitleAliasesController : ControllerBase
{
    private readonly ITitleAliasService _titleAliasService;

    public TitleAliasesController(ITitleAliasService titleAliasService)
    {
        _titleAliasService = titleAliasService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTitleAliases([FromQuery] int? page, [FromQuery] int? pageSize)
    {
        var result = await _titleAliasService.GetAllTitleAliasesAsync(page ?? 0, pageSize ?? 10);
        return Ok(result);
    }

    [HttpGet("{tconst}/{ordering}")]
    public async Task<IActionResult> GetTitleAliasById(string tconst, int ordering)
    {
        var result = await _titleAliasService.GetTitleAliasByIdAsync(tconst, ordering);
        
        if (result == null)
            return NotFound();

        return Ok(result);
    }
}
