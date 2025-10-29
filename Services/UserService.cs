using ImdbClone.Api.Database;
using ImdbClone.Api.DTOs;
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
        
        if (user is null) return null;

        return new UserDto
        {
            UserId = user.UserId,
            Username = user.Username,
            Name = user.Name,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            Salt = user.Salt
        };
    }

    public async Task<UserDto?> GetUserAsync(Guid id)
    {
        var user = await dbContext.ImdbUsers.FirstOrDefaultAsync(u => u.UserId == id);
        
        if (user is null) return null;
        
        return new UserDto
        {
            UserId = user.UserId,
            Username = user.Username,
            Name = user.Name,
            Email = user.Email
        };
    }
    
    public async Task<ImdbUser> CreateUserAsync(string name, string username, string email, string password = null, string salt = null)
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

    public async Task<PaginatedResult<BookmarkTitleListDto>> GetAllBookmarkedTitlesAsync(Guid userId, int page = 0, int pageSize = 10)
    {
        var total = await dbContext.BookmarkTitles.CountAsync(bt => bt.UserId == userId);
        
        var items = await dbContext.BookmarkTitles
            .Where(bt => bt.UserId == userId)
            .Select(bt => new BookmarkTitleListDto
            {
                Tconst = bt.Tconst,
                PrimaryTitle = bt.Title.PrimaryTitle,
                BookmarkDate = bt.BookmarkDate
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
            PageSize = pageSize
        };
    }

    public async Task<bool> CreateBookmarkTitleAsync(Guid userId, string tconst)
    {
        var titleExists = await dbContext.Titles.AnyAsync(t => t.Tconst == tconst);
        
        if (!titleExists) return false;
        
        await dbContext.Database.ExecuteSqlRawAsync("SELECT bookmark_title({0}, {1})", userId, tconst);
        return true;
    }

    public async Task<bool> DeleteBookmarkTitleAsync(Guid userId, string tconst)
    {
        var titleBookmarkExists = await dbContext.BookmarkTitles.AnyAsync(bt => bt.UserId == userId && bt.Tconst == tconst);
        
        if (!titleBookmarkExists) return false;
        
        await dbContext.Database.ExecuteSqlRawAsync("SELECT unbookmark_title({0}, {1})", userId, tconst);
        return true;
    }
    
    public async Task<PaginatedResult<BookmarkPersonListDto>> GetAllBookmarkedPersonAsync(Guid userId, int page = 0, int pageSize = 10)
    {
        var total = await dbContext.BookmarkPeople.CountAsync(bt => bt.UserId == userId);
        
        var items = await dbContext.BookmarkPeople
            .Where(bp => bp.UserId == userId)
            .Select(bp => new BookmarkPersonListDto
            {
                Nconst = bp.Nconst,
                FullName = bp.Person.FullName,
                BookmarkDate = bp.BookmarkDate
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
            PageSize = pageSize
        };
    }

    public async Task<bool> CreateBookmarkPersonAsync(Guid userId, string nconst)
    {
        var personExists = await dbContext.People.AnyAsync(p => p.Nconst == nconst);
        
        if (!personExists) return false;

        await dbContext.Database.ExecuteSqlRawAsync("SELECT bookmark_person({0}, {1})", userId, nconst);
        return true;
    }

    public async Task<bool> DeleteBookmarkPersonAsync(Guid userId, string nconst)
    {
        var personBookmarkExists = await dbContext.BookmarkPeople.AnyAsync(bp => bp.UserId == userId && bp.Nconst == nconst);
        
        if (!personBookmarkExists) return false;

        await dbContext.Database.ExecuteSqlRawAsync("SELECT unbookmark_person({0}, {1})", userId, nconst);
        return true;
    }

    public async Task<PaginatedResult<TitleRatingListDto>> GetAllRatedTitlesAsync(Guid userId, int page = 0,
        int pageSize = 10)
    {
        var total = await dbContext.UserRatings.CountAsync(ur => ur.UserId == userId);

        var items = await dbContext.UserRatings.Where(ur => ur.UserId == userId)
            .Select(ur => new TitleRatingListDto
            {
                Tconst = ur.Tconst,
                PrimaryTitle = ur.Title.PrimaryTitle,
                Rating = ur.Rating,
                RatingDate = ur.RatingDate
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
            PageSize = pageSize
        };
    }

    public async Task<bool> CreateTitleRatingAsync(Guid userId, string tconst, int rating)
    {
        var titleExists = await dbContext.Titles.AnyAsync(t => t.Tconst == tconst);

        if (!titleExists) return false;

        await dbContext.Database.ExecuteSqlRawAsync("SELECT rate({0}, {1}, {2})", userId, tconst, rating);
        return true;
    }

    public async Task<bool> DeleteTitleRatingAsync(string tconst, Guid userId)
    {
        var ratingExists = await dbContext.UserRatings.AnyAsync(t => t.Tconst == tconst && t.UserId == userId);

        if (!ratingExists) return false;

        await dbContext.Database.ExecuteSqlRawAsync("SELECT delete_user_rating({0}, {1})", tconst, userId);
        return true;
    }
}