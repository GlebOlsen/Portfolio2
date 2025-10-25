using ImdbClone.Api.DTOs;
using ImdbClone.Api.Services;

namespace ImdbClone.Api.Interfaces;

public interface ISearchService
{
    Task<PaginatedResult<TitleSearchResultDto>> StructuredSearchAsync(
        Guid userId,
        string? title,
        string? plot,
        string? characters,
        string? preson,
        int page = 0,
        int pageSize = 10
    );
}
