using ImdbClone.Api.Interfaces;
using ImdbClone.Api.Services;
using ImdbClone.Api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ImdbClone.Api.Controllers;

[ApiController]
[Route("persons")]
public class PersonsController(IPersonService personService, PaginationService paginationService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllPersons(
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10
    )
    {
        var result = await personService.GetAllPersonsAsync(page, pageSize);

        var queryParams = new Dictionary<string, string?>();
        paginationService.SetPaginationUrls(result, Request.Path, queryParams);

        return Ok(result);
    }

    [HttpGet("{nconst}")]
    public async Task<IActionResult> GetPersonById(string nconst)
    {
        var userId = User.GetUserId();
        
        var result = await personService.GetPersonByIdAsync(nconst, userId);

        if (result == null)
        {
            return NotFound(new { message = $"Person '{nconst}' not found" });
        }

        return Ok(result);
    }
}
