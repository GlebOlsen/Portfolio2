using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ImdbClone.Api.DTOs.Users;
using ImdbClone.Api.Interfaces;
using ImdbClone.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ImdbClone.Api.Controllers;

[ApiController]
[Route("users")]
public class UsersController(IUserService userService, Hashing hashing, IConfiguration configuration) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SignUp(CreateUserDto dto)
    {
        if (await userService.GetUser(dto.Username) is not null || string.IsNullOrEmpty(dto.Password)) return BadRequest();
        
        var (hashedPwd, salt) = hashing.Hash(dto.Password);

        await userService.CreateUser(dto.Name, dto.Username, dto.Email, hashedPwd, salt);

        return Ok();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto dto)
    {
        var user = await userService.GetUser(dto.Username);

        if(user == null || !hashing.Verify(dto.Password, user.PasswordHash, user.Salt))
        {
            return BadRequest();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var secret = configuration["JWT_SECRET"];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(4),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new { user.Username, token = jwt });
    }

    [HttpGet("bookmark-title")]
    [Authorize]
    public async Task<IActionResult> GetBookmarkedTitles([FromQuery] int? pageSize)
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId)) return BadRequest();

        var result = await userService.GetAllBookmarkedTitlesAsync(userId, pageSize: pageSize ?? 10);

        return Ok(result);
    }

    [HttpPost("bookmark-title")]
    [Authorize]
    public async Task<IActionResult> CreateBookmarkTitle(CreateBookmarkTitleDto dto)
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId)) return BadRequest();
        
        var result = await userService.CreateBookmarkTitle(userId, dto.Tconst);

        if (!result) return NotFound("Title not found");
       
        return Ok();
    }

    [HttpDelete("bookmark-title")]
    [Authorize]
    public async Task<IActionResult> DeleteBookmarkTitle(DeleteBookmarkTitleDto dto)
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId)) return BadRequest();
        
        var result =  await userService.DeleteBookmarkTitle(userId, dto.Tconst);

        if (!result) return NotFound("Title not found");
       
        return NoContent();
    }
}