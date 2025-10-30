using ImdbClone.Api.DTOs;
using ImdbClone.Api.Services;

namespace ImdbClone.Api.Interfaces;

public interface IEpisodeService
{
    Task<PaginatedResult<EpisodeDto>> GetAllEpisodesAsync(int page = 0, int pageSize = 10);
    Task<EpisodeDto?> GetEpisodeByIdAsync(string tconst);
}
