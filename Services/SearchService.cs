using ImdbClone.Api.Database;
using ImdbClone.Api.DTOs;
using ImdbClone.Api.Interfaces;
using ImdbClone.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace IMDB.DataService.Services;

public class SearchService : ISearchService
{
    private readonly ApplicationDbContext _db;

    public SearchService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<PaginatedResult<TitleSearchResultDto>> StructuredSearchAsync(
        Guid userId,
        string? title,
        string? plot,
        string? characters,
        string? person,
        int page = 0,
        int pageSize = 10
    )
    {
        var results = await _db.Set<TitleSearchResultDto>()
            .FromSqlInterpolated(
                $"SELECT * FROM structured_string_search({userId}, {title}, {plot}, {characters}, {person})"
            )
            .ToListAsync();

        results = results.OrderBy(r => r.PrimaryTitle).ToList();

        var pageResults = results.Skip(page * pageSize).Take(pageSize).ToList();

        return new PaginatedResult<TitleSearchResultDto>
        {
            Items = pageResults,
            Total = results.Count,
            Page = page,
            PageSize = pageSize,
        };
    }
}
