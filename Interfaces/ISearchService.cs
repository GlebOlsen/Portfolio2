using IMDB.DataService.DTOs.Title;

namespace IMDB.DataService.Interfaces;

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
