using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ImdbClone.Api.Controllers;
using ImdbClone.Api.Database;
using ImdbClone.Api.DTOs;
using ImdbClone.Api.DTOs.Users;
using ImdbClone.Api.Entities;
using ImdbClone.Api.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ImdbClone.Api.Services;

public class UsersService : IUsersService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly Hashing _hashing;

    public UsersService(
        ApplicationDbContext dbContext,
        IConfiguration configuration,
        Hashing hashing
    )
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _hashing = hashing;
    }

    private async Task<UserDto?> GetUserAsync(string? username)
    {
        var user = await _dbContext.ImdbUsers.FirstOrDefaultAsync(u => u.Username == username);

        if (user is null)
            return null;

        return new UserDto
        {
            UserId = user.UserId,
            Username = user.Username,
            Name = user.Name,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            Salt = user.Salt,
        };
    }

    public async Task<UserResponseDto?> ValidateLoginAsync(string username, string password)
    {
        var user = await GetUserAsync(username);

        if (user == null || !_hashing.Verify(password, user.PasswordHash, user.Salt))
            return null;

        return new UserResponseDto
        {
            UserId = user.UserId,
            Username = user.Username,
            Name = user.Name,
            Email = user.Email,
        };
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _dbContext.ImdbUsers.FirstOrDefaultAsync(u => u.UserId == id);

        if (user is null)
            return null;

        return new UserResponseDto
        {
            UserId = user.UserId,
            Username = user.Username,
            Name = user.Name,
            Email = user.Email,
        };
    }

    public async Task<ImdbUser> CreateUserAsync(
        string name,
        string username,
        string email,
        string password = null,
        string salt = null
    )
    {
        var user = new ImdbUser
        {
            Name = name,
            Username = username,
            Email = email,
            PasswordHash = password,
            Salt = salt,
        };

        await _dbContext.ImdbUsers.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<UserResponseDto?> GetUserResponseAsync(string username)
    {
        var user = await _dbContext.ImdbUsers.FirstOrDefaultAsync(u => u.Username == username);

        if (user is null)
            return null;

        return new UserResponseDto
        {
            UserId = user.UserId,
            Username = user.Username,
            Name = user.Name,
            Email = user.Email,
        };
    }

    public async Task<UserResponseDto?> UpdateUsernameAsync(Guid userId, string username)
    {
        var user = await _dbContext.ImdbUsers.FindAsync(userId);
        if (user == null)
        {
            return null;
        }

        user.Username = username;
        await _dbContext.SaveChangesAsync();

        return new UserResponseDto
        {
            UserId = user.UserId,
            Username = user.Username,
            Name = user.Name,
            Email = user.Email,
        };
    }

    public async Task<PaginatedResult<BookmarkTitleListDto>> GetAllBookmarkedTitlesAsync(
        Guid userId,
        int page = 0,
        int pageSize = 10
    )
    {
        var total = await _dbContext.BookmarkTitles.CountAsync(bt => bt.UserId == userId);

        var items = await _dbContext
            .BookmarkTitles.Where(bt => bt.UserId == userId)
            .Select(bt => new BookmarkTitleListDto
            {
                Tconst = bt.Tconst,
                PrimaryTitle = bt.Title.PrimaryTitle,
                BookmarkDate = bt.BookmarkDate,
            })
            .OrderByDescending(bt => bt.BookmarkDate)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResult<BookmarkTitleListDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize,
        };
    }

    public async Task<bool> CreateBookmarkTitleAsync(Guid userId, string tconst)
    {
        var titleExists = await _dbContext.Titles.AnyAsync(t => t.Tconst == tconst);

        if (!titleExists)
            return false;

        await _dbContext.Database.ExecuteSqlRawAsync(
            "SELECT bookmark_title({0}, {1})",
            userId,
            tconst
        );
        return true;
    }

    public async Task<bool> DeleteBookmarkTitleAsync(Guid userId, string tconst)
    {
        var titleBookmarkExists = await _dbContext.BookmarkTitles.AnyAsync(bt =>
            bt.UserId == userId && bt.Tconst == tconst
        );

        if (!titleBookmarkExists)
            return false;

        await _dbContext.Database.ExecuteSqlRawAsync(
            "SELECT unbookmark_title({0}, {1})",
            userId,
            tconst
        );
        return true;
    }

    public async Task<PaginatedResult<BookmarkPersonListDto>> GetAllBookmarkedPersonAsync(
        Guid userId,
        int page = 0,
        int pageSize = 10
    )
    {
        var total = await _dbContext.BookmarkPeople.CountAsync(bt => bt.UserId == userId);

        var items = await _dbContext
            .BookmarkPeople.Where(bp => bp.UserId == userId)
            .Select(bp => new BookmarkPersonListDto
            {
                Nconst = bp.Nconst,
                FullName = bp.Person.FullName,
                BookmarkDate = bp.BookmarkDate,
            })
            .OrderByDescending(bp => bp.BookmarkDate)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResult<BookmarkPersonListDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize,
        };
    }

    public async Task<bool> CreateBookmarkPersonAsync(Guid userId, string nconst)
    {
        var personExists = await _dbContext.People.AnyAsync(p => p.Nconst == nconst);

        if (!personExists)
            return false;

        await _dbContext.Database.ExecuteSqlRawAsync(
            "SELECT bookmark_person({0}, {1})",
            userId,
            nconst
        );
        return true;
    }

    public async Task<bool> DeleteBookmarkPersonAsync(Guid userId, string nconst)
    {
        var personBookmarkExists = await _dbContext.BookmarkPeople.AnyAsync(bp =>
            bp.UserId == userId && bp.Nconst == nconst
        );

        if (!personBookmarkExists)
            return false;

        await _dbContext.Database.ExecuteSqlRawAsync(
            "SELECT unbookmark_person({0}, {1})",
            userId,
            nconst
        );
        return true;
    }

    public async Task<PaginatedResult<TitleRatingListDto>> GetAllRatedTitlesAsync(
        Guid userId,
        int page = 0,
        int pageSize = 10
    )
    {
        var total = await _dbContext.UserRatings.CountAsync(ur => ur.UserId == userId);

        var items = await _dbContext
            .UserRatings.Where(ur => ur.UserId == userId)
            .Select(ur => new TitleRatingListDto
            {
                Tconst = ur.Tconst,
                PrimaryTitle = ur.Title.PrimaryTitle,
                Rating = ur.Rating,
                RatingDate = ur.RatingDate,
            })
            .OrderByDescending(ur => ur.RatingDate)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResult<TitleRatingListDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize,
        };
    }

    public async Task<bool> CreateTitleRatingAsync(Guid userId, string tconst, int rating)
    {
        var titleExists = await _dbContext.Titles.AnyAsync(t => t.Tconst == tconst);

        if (!titleExists)
            return false;

        await _dbContext.Database.ExecuteSqlRawAsync(
            "SELECT rate({0}, {1}, {2})",
            userId,
            tconst,
            rating
        );
        return true;
    }

    public async Task<bool> DeleteTitleRatingAsync(string tconst, Guid userId)
    {
        var ratingExists = await _dbContext.UserRatings.AnyAsync(t =>
            t.Tconst == tconst && t.UserId == userId
        );

        if (!ratingExists)
            return false;

        await _dbContext.Database.ExecuteSqlRawAsync(
            "SELECT delete_user_rating({0}, {1})",
            tconst,
            userId
        );
        return true;
    }

    public async Task<PaginatedResult<SearchHistoryListDto>> GetAllSearchHistoryAsync(
        Guid userId,
        int page = 0,
        int pageSize = 10
    )
    {
        var total = await _dbContext.SearchHistories.CountAsync(sh => sh.UserId == userId);

        var items = await _dbContext
            .SearchHistories.Where(sh => sh.UserId == userId)
            .Select(sh => new SearchHistoryListDto()
            {
                SearchDate = sh.SearchDate,
                SearchParameters = sh.SearchParameters,
            })
            .OrderByDescending(sh => sh.SearchDate)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResult<SearchHistoryListDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize,
        };
    }

    public async Task<bool> DeleteAllSearchHistoryAsync(Guid userId)
    {
        var searchHistoryExists = await _dbContext.SearchHistories.AnyAsync(sh =>
            sh.UserId == userId
        );

        if (!searchHistoryExists)
            return false;

        await _dbContext.Database.ExecuteSqlRawAsync("SELECT clear_search_history({0})", userId);
        return true;
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        var user = await _dbContext.ImdbUsers.FindAsync(userId);
        if (user == null)
        {
            return false;
        }

        var userRatings = await _dbContext.UserRatings
            .Where(ur => ur.UserId == userId)
            .ToListAsync();

        if (userRatings.Count > 0)
        {
            _dbContext.UserRatings.RemoveRange(userRatings);
        }

        var userTitleBookmarks = await _dbContext.BookmarkTitles
            .Where(bt => bt.UserId == userId)
            .ToListAsync();

        if (userTitleBookmarks.Count > 0)
        {
            _dbContext.BookmarkTitles.RemoveRange(userTitleBookmarks);
        }

        var userPersonBookmarks = await _dbContext.BookmarkPeople
            .Where(bp => bp.UserId == userId)
            .ToListAsync();

        if (userPersonBookmarks.Count > 0)
        {
            _dbContext.BookmarkPeople.RemoveRange(userPersonBookmarks);
        }

        var userSearchHistory = await _dbContext.SearchHistories
            .Where(sh => sh.UserId == userId)
            .ToListAsync();

        if (userSearchHistory.Count > 0)
        {
            _dbContext.SearchHistories.RemoveRange(userSearchHistory);
        }

        _dbContext.ImdbUsers.Remove(user);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public string GenerateJwtToken(UserResponseDto user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
        };

        var secret = _configuration["JWT_SECRET"];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(4),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
