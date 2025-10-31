using ImdbClone.Api.Database;
using ImdbClone.Api.DTOs;
using ImdbClone.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImdbClone.Api.Services;

public class TitleService : ITitleService
{
    private readonly ApplicationDbContext _db;

    public TitleService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<PaginatedResult<TitleListDto>> GetAllTitlesAsync(
        int page = 0,
        int pageSize = 10
    )
    {
        var total = await _db.Titles.CountAsync();

        var items = await _db
            .Titles.OrderBy(t => t.PrimaryTitle)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(t => new TitleListDto
            {
                Tconst = t.Tconst,
                PrimaryTitle = t.PrimaryTitle,
                StartYear = t.StartYear,
                Plot = t.Plot,
                Type = t.TitleType,
                PosterUrl = t.Poster,
            })
            .ToListAsync();

        return new PaginatedResult<TitleListDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize,
        };
    }

    public async Task<TitleFullDto?> GetTitleByIdAsync(string tconst)
    {
        return await _db
            .Titles.Where(t => t.Tconst == tconst)
            .Select(t => new TitleFullDto
            {
                Tconst = t.Tconst,
                PrimaryTitle = t.PrimaryTitle,
                OriginalTitle = t.OriginalTitle,
                Type = t.TitleType,
                StartYear = t.StartYear,
                EndYear = t.EndYear,
                RuntimeMin = t.RunTimeMin,
                Rated = t.Rated,
                Plot = t.Plot,
                PosterUrl = t.Poster,
                Award = t.Award,
                Genres = t.Genres.Select(g => g.GenreName).ToList(),
                Countries = t.Countries.Select(c => c.CountryName).ToList(),
                People = t
                    .TitlePeople.Select(tp => new TitlePersonDto
                    {
                        Nconst = tp.Nconst,
                        FullName = tp.Person.FullName,
                        Category = tp.Category,
                        CharacterName = tp.CharacterName,
                    })
                    .ToList(),
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PaginatedResult<TitleListDto>> GetTitlesByGenre(
        string genreName,
        int page = 0,
        int pageSize = 10
    )
    {
        var query = _db.Titles.Where(t =>
            t.Genres.Any(g => g.GenreName.ToLower() == genreName.ToLower())
        );
        int count = await query.CountAsync();

        var items = await query
            .OrderBy(t => t.PrimaryTitle)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(t => new TitleListDto
            {
                Tconst = t.Tconst,
                PrimaryTitle = t.PrimaryTitle,
                StartYear = t.StartYear,
                Plot = t.Plot,
                Type = t.TitleType,
                PosterUrl = t.Poster,
            })
            .ToListAsync();

        return new PaginatedResult<TitleListDto>
        {
            Items = items,
            Total = count,
            Page = page,
            PageSize = pageSize,
        };
    }

    public async Task<PaginatedResult<TitleListDto>> GetTitlesByPersonAsync(
        string nconst,
        int page = 0,
        int pageSize = 10
    )
    {
        var query = _db.Titles.Where(t => t.TitlePeople.Any(p => p.Nconst == nconst));
        var count = await query.CountAsync();

        var items = await query
            .OrderBy(t => t.PrimaryTitle)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(t => new TitleListDto
            {
                Tconst = t.Tconst,
                PrimaryTitle = t.PrimaryTitle,
                StartYear = t.StartYear,
                Plot = t.Plot,
                Type = t.TitleType,
                PosterUrl = t.Poster,
            })
            .ToListAsync();

        return new PaginatedResult<TitleListDto>
        {
            Items = items,
            Total = count,
            Page = page,
            PageSize = pageSize,
        };
    }

    public async Task<PaginatedResult<TitleListDto>> GetTitlesByTypeAsync(
        string titleType,
        int page = 0,
        int pageSize = 10
    )
    {
        var query = _db.Titles.Where(t => t.TitleType.ToString().ToLower() == titleType.ToLower());
        var count = await query.CountAsync();

        var items = await query
            .OrderBy(t => t.PrimaryTitle)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(t => new TitleListDto
            {
                Tconst = t.Tconst,
                PrimaryTitle = t.PrimaryTitle,
                StartYear = t.StartYear,
                Plot = t.Plot,
                Type = t.TitleType,
                PosterUrl = t.Poster,
            })
            .ToListAsync();

        return new PaginatedResult<TitleListDto>
        {
            Items = items,
            Total = count,
            Page = page,
            PageSize = pageSize,
        };
    }

    public async Task<int> GetTotalTitlesCountAsync()
    {
        return await _db.Titles.CountAsync();
    }

    public async Task<List<CountryDto>> GetCountriesByTitleAsync(string tconst)
    {
        return await _db
            .Titles.Where(t => t.Tconst == tconst)
            .SelectMany(t => t.Countries)
            .Select(c => new CountryDto { CountryId = c.CountryId, CountryName = c.CountryName })
            .ToListAsync();
    }

    public async Task<List<GenreDto>> GetGenresByTitleAsync(string tconst)
    {
        return await _db
            .Titles.Where(t => t.Tconst == tconst)
            .SelectMany(t => t.Genres)
            .Select(g => new GenreDto { GenreId = g.GenreId, GenreName = g.GenreName })
            .ToListAsync();
    }
}
