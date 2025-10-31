using ImdbClone.Api.Interfaces;
using ImdbClone.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImdbClone.Api.Controllers;

[ApiController]
[Route("countries")]
public class CountriesController(
    ICountryService countryService,
    PaginationService paginationService
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllCountries(
        [FromQuery] int? page,
        [FromQuery] int? pageSize
    )
    {
        var result = await countryService.GetAllCountriesAsync(page ?? 0, pageSize ?? 10);

        var queryParams = new Dictionary<string, string?>();
        paginationService.SetPaginationUrls(result, Request.Path, queryParams);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCountryById(Guid id)
    {
        var result = await countryService.GetCountryByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}
