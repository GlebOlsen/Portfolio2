using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ImdbClone.Api.DTOs;
using ImdbClone.Api.DTOs.Users;
using ImdbClone.Api.Interfaces;
using ImdbClone.Api.Services;
using ImdbClone.Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ImdbClone.Api.Controllers;

[ApiController]
[Route("users")]
public class UsersController(
    IUserService userService,
    Hashing hashing,
    PaginationService paginationService,
    IConfiguration configuration
) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SignUp(CreateUserDto dto)
    {
        if (
            await userService.GetUserAsync(dto.Username) is not null
            || string.IsNullOrEmpty(dto.Password)
        )
            return BadRequest();

        var (hashedPwd, salt) = hashing.Hash(dto.Password);

        await userService.CreateUserAsync(dto.Name, dto.Username, dto.Email, hashedPwd, salt);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto dto)
    {
        var user = await userService.GetUserAsync(dto.Username);

        if (user == null || !hashing.Verify(dto.Password, user.PasswordHash, user.Salt))
        {
            return BadRequest();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
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
    public async Task<IActionResult> GetAllBookmarkedTitles(
        [FromQuery] int? page = 0,
        [FromQuery] int? pageSize = 10
    )
    {
        var userId = User.GetUserId();

        if (userId is null)
            return BadRequest("Invalid user ID");

        var result = await userService.GetAllBookmarkedTitlesAsync(
            userId.Value,
            page: page ?? 0,
            pageSize: pageSize ?? 10
        );

        var queryParams = new Dictionary<string, string?>();

        paginationService.SetPaginationUrls(result, Request.Path, queryParams);

        return Ok(result);
    }

    [HttpPost("bookmark-title")]
    [Authorize]
    public async Task<IActionResult> CreateBookmarkTitle(CreateBookmarkTitleDto dto)
    {
        var userId = User.GetUserId();

        if (userId is null)
            return BadRequest("Invalid user ID");

        var result = await userService.CreateBookmarkTitleAsync(userId.Value, dto.Tconst);

        if (!result)
            return NotFound("Title not found");

        return Ok();
    }

    [HttpDelete("bookmark-title")]
    [Authorize]
    public async Task<IActionResult> DeleteBookmarkTitle(DeleteBookmarkTitleDto dto)
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            return BadRequest();

        var result = await userService.DeleteBookmarkTitleAsync(userId, dto.Tconst);

        if (!result)
            return NotFound("Title not found");

        return NoContent();
    }

    [HttpGet("bookmark-person")]
    [Authorize]
    public async Task<IActionResult> GetAllBookmarkedPersons(
        [FromQuery] int? page = 0,
        [FromQuery] int? pageSize = 10
    )
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            return BadRequest();

        var result = await userService.GetAllBookmarkedPersonAsync(
            userId,
            page: page ?? 0,
            pageSize: pageSize ?? 10
        );

        var queryParams = new Dictionary<string, string?>();

        paginationService.SetPaginationUrls(result, Request.Path, queryParams);

        return Ok(result);
    }

    [HttpPost("bookmark-person")]
    [Authorize]
    public async Task<IActionResult> CreateBookmarkPerson(CreateBookmarkPersonDto dto)
    {
        var userId = User.GetUserId();

        if (userId is null)
            return BadRequest("Invalid user ID");

        var result = await userService.CreateBookmarkPersonAsync(userId.Value, dto.Nconst);
        if (!result)
            return NotFound("Person not found");

        return Ok();
    }

    [HttpDelete("bookmark-person")]
    [Authorize]
    public async Task<IActionResult> DeleteBookmarkPerson(CreateBookmarkPersonDto dto)
    {
        var userId = User.GetUserId();

        if (userId is null)
            return BadRequest("Invalid user ID");

        var result = await userService.DeleteBookmarkPersonAsync(userId.Value, dto.Nconst);

        if (!result)
            return NotFound("Title not found");

        return Ok();
    }

    [HttpDelete("rate-title")]
    [Authorize]
    public async Task<IActionResult> DeleteTitleRating(DeleteTitleRatingDto dto)
    {
        var userId = User.GetUserId();

        if (userId is null)
            return BadRequest("Invalid user ID");

        var result = await userService.DeleteTitleRatingAsync(dto.Tconst, userId.Value);

        if (!result)
            return NotFound("Person not found");

        return NoContent();
    }

    [HttpPost("rate-title")]
    [Authorize]
    public async Task<IActionResult> CreateTitleRating(CreateTitleRatingDto dto)
    {
        var userId = User.GetUserId();

        if (userId is null)
            return BadRequest("Invalid user ID");

        var result = await userService.CreateTitleRatingAsync(userId.Value, dto.Tconst, dto.Rating);

        if (!result)
            return NotFound("Title not found");

        return Ok();
    }
}
