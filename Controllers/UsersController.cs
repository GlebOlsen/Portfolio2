using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ImdbClone.Api.DTOs;
using ImdbClone.Api.DTOs.Users;
using ImdbClone.Api.Interfaces;
using ImdbClone.Api.Services;
using ImdbClone.Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImdbClone.Api.Controllers;

[ApiController]
[Route("users")]
public class UsersController(
    IUsersService usersService,
    Hashing hashing,
    PaginationService paginationService,
    IConfiguration configuration
) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var tokenString = Request.Cookies["authToken"];
        if (string.IsNullOrEmpty(tokenString))
            return Unauthorized();

        var handler = new JwtSecurityTokenHandler();
        JwtSecurityToken token;

        try
        {
            token = handler.ReadJwtToken(tokenString);
        }
        catch
        {
            return Unauthorized();
        }

        var username = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(username))
            return Unauthorized();

        var user = await usersService.GetUserResponseAsync(username);

        if (user == null)
            return Unauthorized();

        return Ok(new { username = user.Username, email = user.Email });
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(CreateUserDto dto)
    {
        if (
            await usersService.GetUserResponseAsync(dto.Username) is not null
            || string.IsNullOrEmpty(dto.Password)
        )
            return BadRequest();

        var (hashedPwd, salt) = hashing.Hash(dto.Password);
        await usersService.CreateUserAsync(dto.Name, dto.Username, dto.Email, hashedPwd, salt);

        var user = await usersService.GetUserResponseAsync(dto.Username);
        if (user == null)
            return Unauthorized(new { message = "User creation failed" });

        var jwt = usersService.GenerateJwtToken(user);

        Response.Cookies.Append(
            "authToken",
            jwt,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(7),
                Path = "/",
            }
        );

        return Ok(new { username = user.Username, email = user.Email });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
    {
        var user = await usersService.ValidateLoginAsync(dto.Username, dto.Password);

        if (user == null)
            return Unauthorized(new { message = "Invalid username or password" });

        var jwt = usersService.GenerateJwtToken(user);

        Response.Cookies.Append(
            "authToken",
            jwt,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(7),
                Path = "/",
            }
        );

        return Ok(new { username = user.Username, email = user.Email });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("authToken");
        return Ok(new { message = "Logged out successfully" });
    }

    [HttpPatch("update-username")]
    [Authorize]
    public async Task<IActionResult> UpdateUsername([FromBody] UpdateUsernameDto dto)
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        var result = await usersService.UpdateUsernameAsync(userId.Value, dto.Username);
        if (result == null)
        {
            return NotFound();
        }

        var jwt = usersService.GenerateJwtToken(result);

        return Ok(new { result.Username, jwt });
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteUser()
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        var result = await usersService.DeleteUserAsync(userId.Value);
        if (!result)
        {
            return NotFound();
        }

        return Ok();
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

        var result = await usersService.GetAllBookmarkedTitlesAsync(
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

        var result = await usersService.CreateBookmarkTitleAsync(userId.Value, dto.Tconst);

        if (!result)
            return NotFound("Title not found");

        return Ok();
    }

    [HttpDelete("bookmark-title")]
    [Authorize]
    public async Task<IActionResult> DeleteBookmarkTitle(DeleteBookmarkTitleDto dto)
    {
        var userId = User.GetUserId();

        if (userId is null)
            return BadRequest("Invalid user ID");

        var result = await usersService.DeleteBookmarkTitleAsync(userId.Value, dto.Tconst);

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
        var userId = User.GetUserId();

        if (userId is null)
            return BadRequest("Invalid user ID");

        var result = await usersService.GetAllBookmarkedPersonAsync(
            userId.Value,
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

        var result = await usersService.CreateBookmarkPersonAsync(userId.Value, dto.Nconst);
        if (!result)
            return NotFound("Person not found");

        return Ok();
    }

    [HttpDelete("bookmark-person")]
    [Authorize]
    public async Task<IActionResult> DeleteBookmarkPerson(DeleteBookmarkPersonDto dto)
    {
        var userId = User.GetUserId();

        if (userId is null)
            return BadRequest("Invalid user ID");

        var result = await usersService.DeleteBookmarkPersonAsync(userId.Value, dto.Nconst);

        if (!result)
            return NotFound("Title not found");

        return Ok();
    }

    [HttpGet("rate-title")]
    [Authorize]
    public async Task<IActionResult> GetAllRatedTitles(
        [FromQuery] int? page = 0,
        [FromQuery] int? pageSize = 10
    )
    {
        var userId = User.GetUserId();

        if (userId is null)
            return BadRequest("Invalid user ID");

        var result = await usersService.GetAllRatedTitlesAsync(
            userId.Value,
            page: page ?? 0,
            pageSize: pageSize ?? 10
        );

        var queryParams = new Dictionary<string, string?>();

        paginationService.SetPaginationUrls(result, Request.Path, queryParams);

        return Ok(result);
    }

    [HttpDelete("rate-title")]
    [Authorize]
    public async Task<IActionResult> DeleteTitleRating(DeleteTitleRatingDto dto)
    {
        var userId = User.GetUserId();

        if (userId is null)
            return BadRequest("Invalid user ID");

        var result = await usersService.DeleteTitleRatingAsync(dto.Tconst, userId.Value);

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

        var result = await usersService.CreateTitleRatingAsync(
            userId.Value,
            dto.Tconst,
            dto.Rating
        );

        if (!result)
            return NotFound("Title not found");

        return Ok();
    }

    [HttpGet("search-history")]
    [Authorize]
    public async Task<IActionResult> GetAllSearchHistory(
        [FromQuery] int? page = 0,
        [FromQuery] int? pageSize = 10
    )
    {
        var userId = User.GetUserId();

        if (userId is null)
            return BadRequest("Invalid user ID");

        var result = await usersService.GetAllSearchHistoryAsync(
            userId.Value,
            page: page ?? 0,
            pageSize: pageSize ?? 10
        );

        var queryParams = new Dictionary<string, string?>();

        paginationService.SetPaginationUrls(result, Request.Path, queryParams);

        return Ok(result);
    }

    [HttpDelete("search-history")]
    [Authorize]
    public async Task<IActionResult> DeleteAllSearchHistory()
    {
        var userId = User.GetUserId();

        if (userId is null)
            return BadRequest("Invalid user ID");

        var result = await usersService.DeleteAllSearchHistoryAsync(userId.Value);

        if (!result)
            return NotFound("Search history is empty.");

        return NoContent();
    }
}
