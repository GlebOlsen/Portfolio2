using ImdbClone.Api.DTOs.Users;
using ImdbClone.Api.Entities;
using ImdbClone.Api.Services;

namespace ImdbClone.Api.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetUser(string? username);
    Task<UserDto?> GetUser(Guid id);
    Task<ImdbUser> CreateUser(string name, string username, string email, string password = null, string salt = null);
    Task<PaginatedResult<BookmarkTitleListDto>> GetAllBookmarkedTitlesAsync(Guid userId, int page = 0, int pageSize = 10);
    Task<bool> CreateBookmarkTitle(Guid userId, string tconst);
    Task<bool> DeleteBookmarkTitle(Guid userId, string tconst);
}