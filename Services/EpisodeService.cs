using ImdbClone.Api.Database;
using ImdbClone.Api.DTOs;
using ImdbClone.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImdbClone.Api.Services;

public class EpisodeService : IEpisodeService
{
    private readonly ApplicationDbContext _db;

    public EpisodeService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<PaginatedResult<EpisodeDto>> GetAllEpisodesAsync(int page = 0, int pageSize = 10)
    {
        var total = await _db.Episodes.CountAsync();

        var items = await _db.Episodes
            .OrderBy(e => e.Tconst)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(e => new EpisodeDto
            {
                Tconst = e.Tconst,
                ParentTconst = e.ParentTconst,
                EpisodeNumber = e.EpisodeNumber,
                SeasonNumber = e.SeasonNumber,
                ParentTitle = e.ParentTitle != null ? e.ParentTitle.PrimaryTitle : null
            })
            .ToListAsync();

        return new PaginatedResult<EpisodeDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<EpisodeDto?> GetEpisodeByIdAsync(string tconst)
    {
        return await _db.Episodes
            .Where(e => e.Tconst == tconst)
            .Select(e => new EpisodeDto
            {
                Tconst = e.Tconst,
                ParentTconst = e.ParentTconst,
                EpisodeNumber = e.EpisodeNumber,
                SeasonNumber = e.SeasonNumber,
                ParentTitle = e.ParentTitle != null ? e.ParentTitle.PrimaryTitle : null
            })
            .FirstOrDefaultAsync();
    }
}
