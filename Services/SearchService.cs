using ImdbClone.Api.Database;
using ImdbClone.Api.DTOs;
using ImdbClone.Api.Interfaces;
using ImdbClone.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace ImdbClone.DataService.Services;

public class SearchService : ISearchService
{
    private readonly ApplicationDbContext _db;

    public SearchService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<PaginatedResult<TitleSearchResultDto>> StringSearch(
        string query,
        int page = 0,
        int pageSize = 10
    )
    {
        var allResults = await _db.Set<TitleSearchResultDto>()
            .FromSqlInterpolated($"SELECT * FROM string_search({query})")
            .ToListAsync();

        var total = allResults.Count;

        var items = allResults.Skip(page * pageSize).Take(pageSize).ToList();

        return new PaginatedResult<TitleSearchResultDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize,
        };
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
        var allResults = await _db.Set<TitleSearchResultDto>()
            .FromSqlInterpolated(
                $"SELECT * FROM structured_string_search({userId}, {title}, {plot}, {characters}, {person})"
            )
            .ToListAsync();

        var total = allResults.Count;

        var items = allResults.Skip(page * pageSize).Take(pageSize).ToList();

        return new PaginatedResult<TitleSearchResultDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize,
        };
    }

    public async Task<PaginatedResult<PersonSearchResultDto>> FindNames(
        Guid userId,
        string query,
        int page = 0,
        int pageSize = 10
    )
    {
        var allResults = await _db.Set<PersonSearchResultDto>()
            .FromSqlInterpolated($"SELECT * FROM find_names({userId}, {query})")
            .ToListAsync();

        var total = allResults.Count;

        var items = allResults.Skip(page * pageSize).Take(pageSize).ToList();

        return new PaginatedResult<PersonSearchResultDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize,
        };
    }

    public async Task<PaginatedResult<PersonWithProfessionDto>> FindNamesByProfession(
        Guid userId,
        string name,
        string? profession = null,
        int page = 0,
        int pageSize = 10
    )
    {
        var allResults = await _db.Set<PersonWithProfessionDto>()
            .FromSqlInterpolated(
                $"SELECT * FROM find_names_by_profession({userId}, {name}, {profession})"
            )
            .ToListAsync();

        var total = allResults.Count;

        var items = allResults.Skip(page * pageSize).Take(pageSize).ToList();

        return new PaginatedResult<PersonWithProfessionDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize,
        };
    }

    public async Task<PaginatedResult<TitleSearchResultDto>> SearchTitlesExact(
        List<string> words,
        int page = 0,
        int pageSize = 10
    )
    {
        var lowercaseWords = words.ConvertAll(w => w.ToLowerInvariant());

        var allResults = await _db.Set<TitleSearchResultDto>()
            .FromSqlInterpolated($"SELECT * FROM search_titles_exact({lowercaseWords})")
            .ToListAsync();

        var total = allResults.Count;
        var items = allResults.Skip(page * pageSize).Take(pageSize).ToList();

        return new PaginatedResult<TitleSearchResultDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize,
        };
    }

    public async Task<PaginatedResult<TitleSearchResultDto>> SearchTitlesBestMatch(
        List<string> words,
        int page = 0,
        int pageSize = 10
    )
    {
        var allResults = await _db.Set<TitleSearchResultDto>()
            .FromSqlInterpolated($"SELECT * FROM search_titles_best_match({words})")
            .ToListAsync();

        var total = allResults.Count;
        var items = allResults.Skip(page * pageSize).Take(pageSize).ToList();

        return new PaginatedResult<TitleSearchResultDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize,
        };
    }

    public async Task<PaginatedResult<WordFrequencyDto>> SearchWordsToWords(
        List<string> words,
        int page = 0,
        int pageSize = 10
    )
    {
        var allResults = await _db.Set<WordFrequencyDto>()
            .FromSqlInterpolated($"SELECT * FROM search_words_to_words({words})")
            .ToListAsync();

        var total = allResults.Count;
        var items = allResults.Skip(page * pageSize).Take(pageSize).ToList();

        return new PaginatedResult<WordFrequencyDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize,
        };
    }
}
