using ImdbClone.Api.Database;
using ImdbClone.Api.DTOs;
using ImdbClone.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImdbClone.Api.Services;

public class CountryService : ICountryService
{
    private readonly ApplicationDbContext _db;

    public CountryService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<PaginatedResult<CountryDto>> GetAllCountriesAsync(int page = 0, int pageSize = 10)
    {
        var total = await _db.Countries.CountAsync();

        var items = await _db.Countries
            .OrderBy(c => c.CountryName)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(c => new CountryDto
            {
                CountryId = c.CountryId,
                CountryName = c.CountryName
            })
            .ToListAsync();

        return new PaginatedResult<CountryDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<CountryDto?> GetCountryByIdAsync(Guid countryId)
    {
        return await _db.Countries
            .Where(c => c.CountryId == countryId)
            .Select(c => new CountryDto
            {
                CountryId = c.CountryId,
                CountryName = c.CountryName
            })
            .FirstOrDefaultAsync();
    }
}
