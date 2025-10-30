using ImdbClone.Api.DTOs;
using ImdbClone.Api.Services;

namespace ImdbClone.Api.Interfaces;

public interface IGenreService
{
    Task<PaginatedResult<GenreDto>> GetAllGenresAsync(int page = 0, int pageSize = 10);
    Task<GenreDto?> GetGenreByIdAsync(Guid genreId);
}
