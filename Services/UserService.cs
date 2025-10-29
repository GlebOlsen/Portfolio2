using ImdbClone.Api.Database;
using ImdbClone.Api.DTOs.Users;
using ImdbClone.Api.Entities;
using ImdbClone.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImdbClone.Api.Services;

public class UserService(ApplicationDbContext dbContext) : IUserService
{
    public async Task<UserDto?> GetUserAsync(string? username)
    {
        var user = await dbContext.ImdbUsers.FirstOrDefaultAsync(u => u.Username == username);

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

    public async Task<UserDto?> GetUserAsync(Guid id)
    {
        var user = await dbContext.ImdbUsers.FirstOrDefaultAsync(u => u.UserId == id);

        if (user is null)
            return null;

        return new UserDto
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

        await dbContext.ImdbUsers.AddAsync(user);
        await dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<PaginatedResult<BookmarkTitleListDto>> GetAllBookmarkedTitlesAsync(
        Guid userId,
        int page = 0,
        int pageSize = 10
    )
    {
        var total = await dbContext.BookmarkTitles.CountAsync(bt => bt.UserId == userId);

        var items = await dbContext
            .BookmarkTitles.Where(bt => bt.UserId == userId)
            .Select(bt => new BookmarkTitleListDto
            {
                Tconst = bt.Tconst,
                PrimaryTitle = bt.Title.PrimaryTitle,
                BookmarkDate = bt.BookmarkDate,
            })
            .OrderBy(bt => bt.PrimaryTitle)
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
        var titleExists = await dbContext.Titles.AnyAsync(t => t.Tconst == tconst);

        if (!titleExists)
            return false;

        await dbContext.Database.ExecuteSqlRawAsync(
            "SELECT bookmark_title({0}, {1})",
            userId,
            tconst
        );
        return true;
    }

    public async Task<bool> DeleteBookmarkTitleAsync(Guid userId, string tconst)
    {
        var titleExists = await dbContext.Titles.AnyAsync(t => t.Tconst == tconst);

        if (!titleExists)
            return false;

        await dbContext.Database.ExecuteSqlRawAsync(
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
        var total = await dbContext.BookmarkPeople.CountAsync(bt => bt.UserId == userId);

        var items = await dbContext
            .BookmarkPeople.Where(bp => bp.UserId == userId)
            .Select(bp => new BookmarkPersonListDto
            {
                Nconst = bp.Nconst,
                FullName = bp.Person.FullName,
                BookmarkDate = bp.BookmarkDate,
            })
            .OrderBy(bp => bp.FullName)
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
        var personExists = await dbContext.People.AnyAsync(p => p.Nconst == nconst);

        if (!personExists)
            return false;

        await dbContext.Database.ExecuteSqlRawAsync(
            "SELECT bookmark_person({0}, {1})",
            userId,
            nconst
        );
        return true;
    }

    public async Task<bool> DeleteBookmarkPersonAsync(Guid userId, string nconst)
    {
        var personExists = await dbContext.People.AnyAsync(p => p.Nconst == nconst);

        if (!personExists)
            return false;

        await dbContext.Database.ExecuteSqlRawAsync(
            "SELECT unbookmark_person({0}, {1})",
            userId,
            nconst
        );
        return true;
    }
}
