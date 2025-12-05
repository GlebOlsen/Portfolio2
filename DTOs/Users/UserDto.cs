namespace ImdbClone.Api.DTOs.Users;

public class UserDto
{
    public Guid UserId { get; init; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
}

public class UserResponseDto
{
    public Guid UserId { get; init; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

public record UpdateUsernameDto(string Username);
