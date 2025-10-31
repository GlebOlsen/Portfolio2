using ImdbClone.Api.Database;
using ImdbClone.Api.DTOs;
using ImdbClone.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImdbClone.Api.Services;

public class GenreService : IGenreService
{
    private readonly ApplicationDbContext _db;

    public GenreService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<PaginatedResult<GenreDto>> GetAllGenresAsync(int page = 0, int pageSize = 10)
    {
        var total = await _db.Genres.CountAsync();

        var items = await _db
            .Genres.OrderBy(g => g.GenreName)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(g => new GenreDto { GenreId = g.GenreId, GenreName = g.GenreName })
            .ToListAsync();

        return new PaginatedResult<GenreDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize,
        };
    }

    public async Task<GenreDto?> GetGenreByIdAsync(Guid genreId)
    {
        return await _db
            .Genres.Where(g => g.GenreId == genreId)
            .Select(g => new GenreDto { GenreId = g.GenreId, GenreName = g.GenreName })
            .FirstOrDefaultAsync();
    }
}
