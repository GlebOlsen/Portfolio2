using ImdbClone.Api.DTOs.Users;
using ImdbClone.Api.Entities;

namespace ImdbClone.Api.Interfaces;

public interface IUserService
{
    UserDto? GetUser(string? username);
    UserDto? GetUser(Guid id);
    ImdbUser CreateUser(string name, string username, string email, string password = null, string salt = null);
}