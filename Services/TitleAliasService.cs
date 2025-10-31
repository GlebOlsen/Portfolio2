using ImdbClone.Api.Database;
using ImdbClone.Api.DTOs;
using ImdbClone.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImdbClone.Api.Services;

public class TitleAliasService : ITitleAliasService
{
    private readonly ApplicationDbContext _db;

    public TitleAliasService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<PaginatedResult<TitleAliasDto>> GetAllTitleAliasesAsync(int page = 0, int pageSize = 10)
    {
        var total = await _db.TitleAliases.CountAsync();

        var items = await _db.TitleAliases
            .OrderBy(ta => ta.Tconst)
            .ThenBy(ta => ta.Ordering)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(ta => new TitleAliasDto
            {
                Tconst = ta.Tconst,
                Ordering = ta.Ordering,
                AltTitle = ta.AltTitle,
                Types = ta.Types,
                Language = ta.Language,
                Region = ta.Region,
                Attributes = ta.Attributes
            })
            .ToListAsync();

        return new PaginatedResult<TitleAliasDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<TitleAliasDto?> GetTitleAliasByIdAsync(string tconst, int ordering)
    {
        return await _db.TitleAliases
            .Where(ta => ta.Tconst == tconst && ta.Ordering == ordering)
            .Select(ta => new TitleAliasDto
            {
                Tconst = ta.Tconst,
                Ordering = ta.Ordering,
                AltTitle = ta.AltTitle,
                Types = ta.Types,
                Language = ta.Language,
                Region = ta.Region,
                Attributes = ta.Attributes
            })
            .FirstOrDefaultAsync();
    }
}
