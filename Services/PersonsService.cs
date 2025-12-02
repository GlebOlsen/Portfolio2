using System.Net.Http.Headers;
using ImdbClone.Api.Database;
using ImdbClone.Api.DTOs;
using ImdbClone.Api.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ImdbClone.Api.Services;

public class PersonService : IPersonService
{
    private readonly ApplicationDbContext _db;

    public PersonService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<PaginatedResult<PersonListDto>> GetAllPersonsAsync(
        int page = 0,
        int pageSize = 10
    )
    {
        var total = await _db.People.CountAsync();

        var items = await _db
            .People.OrderBy(p => p.FullName)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(p => new PersonListDto
            {
                Nconst = p.Nconst,
                FullName = p.FullName,
                Professions = p.Professions.Select(prof => prof.ProfessionName).ToList(),
            })
            .ToListAsync();

        return new PaginatedResult<PersonListDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize,
        };
    }

    public async Task<PersonFullDto?> GetPersonByIdAsync(string nconst)
    {
        return await _db
            .People.Where(p => p.Nconst == nconst)
            .Select(p => new PersonFullDto
            {
                Nconst = p.Nconst,
                FullName = p.FullName,
                BirthYear = p.BirthYear,
                DeathYear = p.DeathYear,
                DerivedRating = p.DerivedRating,
                Professions = p.Professions.ToList(),
            })
            .FirstOrDefaultAsync();
    }
}
