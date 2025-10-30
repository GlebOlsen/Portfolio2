using ImdbClone.Api.DTOs;
using ImdbClone.Api.Services;

namespace ImdbClone.Api.Interfaces;

public interface ITitleAliasService
{
    Task<PaginatedResult<TitleAliasDto>> GetAllTitleAliasesAsync(int page = 0, int pageSize = 10);
    Task<TitleAliasDto?> GetTitleAliasByIdAsync(string tconst, int ordering);
}
