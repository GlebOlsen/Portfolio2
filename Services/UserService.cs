using ImdbClone.Api.Database;
using ImdbClone.Api.DTOs.Users;
using ImdbClone.Api.Entities;
using ImdbClone.Api.Interfaces;

namespace ImdbClone.Api.Services;

public class UserService(ApplicationDbContext dbContext) : IUserService
{
    private readonly List<ImdbUser> _users = dbContext.ImdbUsers.ToList();
    
    public UserDto? GetUser(string? username)
    {
        var user = _users.FirstOrDefault(u => u.Username == username);
        
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

    public UserDto? GetUser(Guid id)
    {
        var user = _users.FirstOrDefault(u => u.UserId == id);
        
        if (user is null) return null;
        
        return new UserDto
        {
            UserId = user.UserId,
            Username = user.Username,
            Name = user.Name,
            Email = user.Email
        };
    }
    
    public ImdbUser CreateUser(string name, string username, string email, string password = null, string salt = null)
    {
        var user = new ImdbUser
        {
            Name = name,
            Username = username,
            Email = email,
            PasswordHash = password,
            Salt = salt,
        };

        dbContext.ImdbUsers.Add(user);
        dbContext.SaveChanges();
        return user;
    }
    
}