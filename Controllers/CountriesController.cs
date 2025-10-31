using ImdbClone.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ImdbClone.Api.Controllers;

[ApiController]
[Route("countries")]
public class CountriesController : ControllerBase
{
    private readonly ICountryService _countryService;

    public CountriesController(ICountryService countryService)
    {
        _countryService = countryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCountries([FromQuery] int? page, [FromQuery] int? pageSize)
    {
        var result = await _countryService.GetAllCountriesAsync(page ?? 0, pageSize ?? 10);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCountryById(Guid id)
    {
        var result = await _countryService.GetCountryByIdAsync(id);
        
        if (result == null)
            return NotFound();

        return Ok(result);
    }
}
