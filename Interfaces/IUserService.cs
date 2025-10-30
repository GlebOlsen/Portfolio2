using ImdbClone.Api.DTOs;
using ImdbClone.Api.DTOs.Users;
using ImdbClone.Api.Entities;
using ImdbClone.Api.Services;

namespace ImdbClone.Api.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetUserAsync(string? username);
    Task<UserDto?> GetUserAsync(Guid id);
    Task<ImdbUser> CreateUserAsync(
        string name,
        string username,
        string email,
        string password = null,
        string salt = null
    );
    Task<PaginatedResult<BookmarkTitleListDto>> GetAllBookmarkedTitlesAsync(
        Guid userId,
        int page = 0,
        int pageSize = 10
    );
    Task<bool> CreateBookmarkTitleAsync(Guid userId, string tconst);
    Task<bool> DeleteBookmarkTitleAsync(Guid userId, string tconst);
    Task<PaginatedResult<BookmarkPersonListDto>> GetAllBookmarkedPersonAsync(
        Guid userId,
        int page = 0,
        int pageSize = 10
    );
    Task<bool> CreateBookmarkPersonAsync(Guid userId, string nconst);
    Task<bool> DeleteBookmarkPersonAsync(Guid userId, string nconst);
    Task<PaginatedResult<TitleRatingListDto>> GetAllRatedTitlesAsync(
        Guid userId,
        int page = 0,
        int pageSize = 10
    );
    Task<bool> CreateTitleRatingAsync(Guid userId, string tconst, int rating);
    Task<bool> DeleteTitleRatingAsync(string tconst, Guid userId);
}
