using ImdbClone.Api.DTOs;
using ImdbClone.Api.Services;

namespace ImdbClone.Api.Interfaces;

public interface IRatingService
{
    Task<PaginatedResult<RatingDto>> GetAllRatingsAsync(int page = 0, int pageSize = 10);
    Task<RatingDto?> GetRatingByIdAsync(string tconst);
}
