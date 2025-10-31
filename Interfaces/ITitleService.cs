using ImdbClone.Api.DTOs;
using ImdbClone.Api.Services;

namespace ImdbClone.Api.Interfaces;

public interface ITitleService
{
    Task<PaginatedResult<TitleListDto>> GetAllTitlesAsync(int page = 0, int pageSize = 10);
    Task<TitleFullDto?> GetTitleByIdAsync(string tconst);

    Task<PaginatedResult<TitleListDto>> GetTitlesByGenre(string genreName, int page = 0, int pageSize = 10);
    Task<PaginatedResult<TitleListDto>> GetTitlesByPersonAsync(string nconst, int page = 0, int pageSize = 10);
    Task<PaginatedResult<TitleListDto>> GetTitlesByTypeAsync(string titleType, int page = 0, int pageSize = 10);

    Task<List<CountryDto>> GetCountriesByTitleAsync(string tconst);
    Task<List<GenreDto>> GetGenresByTitleAsync(string tconst);

    Task<int> GetTotalTitlesCountAsync();
}