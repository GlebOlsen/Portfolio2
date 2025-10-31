using ImdbClone.Api.DTOs;
using ImdbClone.Api.Services;

namespace ImdbClone.Api.Interfaces;

public interface ICountryService
{
    Task<PaginatedResult<CountryDto>> GetAllCountriesAsync(int page = 0, int pageSize = 10);
    Task<CountryDto?> GetCountryByIdAsync(Guid countryId);
}
