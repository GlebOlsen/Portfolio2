using ImdbClone.Api.DTOs;
using ImdbClone.Api.Services;

namespace ImdbClone.Api.Interfaces;

public interface IPersonService
{
    Task<PaginatedResult<PersonListDto>> GetAllPersonsAsync(int page = 0, int pageSize = 10);
    Task<PersonFullDto?> GetPersonByIdAsync(string nconst);
}
