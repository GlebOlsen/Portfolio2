using ImdbClone.Api.Database;
using ImdbClone.Api.DTOs;
using ImdbClone.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImdbClone.Api.Services;

public class RatingService : IRatingService
{
    private readonly ApplicationDbContext _db;

    public RatingService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<PaginatedResult<RatingDto>> GetAllRatingsAsync(int page = 0, int pageSize = 10)
    {
        var total = await _db.Ratings.CountAsync();

        var items = await _db.Ratings
            .OrderByDescending(r => r.AvgRating)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(r => new RatingDto
            {
                Tconst = r.Tconst,
                AvgRating = r.AvgRating,
                NumVotes = r.NumVotes,
                MetaScore = r.MetaScore,
                TitleName = r.Title != null ? r.Title.PrimaryTitle : null
            })
            .ToListAsync();

        return new PaginatedResult<RatingDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<RatingDto?> GetRatingByIdAsync(string tconst)
    {
        return await _db.Ratings
            .Where(r => r.Tconst == tconst)
            .Select(r => new RatingDto
            {
                Tconst = r.Tconst,
                AvgRating = r.AvgRating,
                NumVotes = r.NumVotes,
                MetaScore = r.MetaScore,
                TitleName = r.Title != null ? r.Title.PrimaryTitle : null
            })
            .FirstOrDefaultAsync();
    }
}
