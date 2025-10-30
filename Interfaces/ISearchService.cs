using ImdbClone.Api.DTOs;
using ImdbClone.Api.Enums;
using ImdbClone.Api.Services;

namespace ImdbClone.Api.Interfaces;

public interface ISearchService
{
    Task<PaginatedResult<TitleSearchResultDto>> StringSearch(
        string query,
        int page = 0,
        int pageSize = 10
    );

    Task<PaginatedResult<TitleSearchResultDto>> StructuredSearchAsync(
        Guid userId,
        string? title,
        string? plot,
        string? characters,
        string? person,
        int page = 0,
        int pageSize = 10
    );

    Task<PaginatedResult<PersonSearchResultDto>> FindNames(
        Guid userId,
        string query,
        int page = 0,
        int pageSize = 10
    );

    Task<PaginatedResult<PersonWithProfessionDto>> FindNamesByProfession(
        Guid userId,
        string name,
        string? profession = null,
        int page = 0,
        int pageSize = 10
    );

    Task<PaginatedResult<TitleSearchResultDto>> SearchTitlesExact(
        List<string> words,
        int page = 0,
        int pageSize = 10
    );

    Task<PaginatedResult<TitleSearchResultDto>> SearchTitlesBestMatch(
        List<string> words,
        int page = 0,
        int pageSize = 10
    );

    Task<PaginatedResult<WordFrequencyDto>> SearchWordsToWords(
        List<string> words,
        int page = 0,
        int pageSize = 10
    );
}
