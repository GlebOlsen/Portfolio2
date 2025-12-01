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
    IUserService userService,
    Hashing hashing,
    PaginationService paginationService,
    IConfiguration configuration
) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(CreateUserDto dto)
    {
        if (
            await userService.GetUserAsync(dto.Username) is not null
            || string.IsNullOrEmpty(dto.Password)
        )
            return BadRequest();

        var (hashedPwd, salt) = hashing.Hash(dto.Password);
        await userService.CreateUserAsync(dto.Name, dto.Username, dto.Email, hashedPwd, salt);

        var user = await userService.GetUserAsync(dto.Username);
        if (user == null)
            return Unauthorized(new { message = "User creation failed" });

        var jwt = userService.GenerateJwtToken(user);
        return Ok(new { user.Username, token = jwt });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto dto)
    {
        var user = await userService.GetUserAsync(dto.Username);

        if (user == null || !hashing.Verify(dto.Password, user.PasswordHash, user.Salt))
            return Unauthorized(new { message = "Invalid username or password" });

        var jwt = userService.GenerateJwtToken(user);
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
        var userId = User.GetUserId();

        if (userId is null)
            return BadRequest("Invalid user ID");

        var result = await userService.DeleteBookmarkTitleAsync(userId.Value, dto.Tconst);

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

        var result = await userService.GetAllBookmarkedPersonAsync(
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

        var result = await userService.CreateBookmarkPersonAsync(userId.Value, dto.Nconst);
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

        var result = await userService.DeleteBookmarkPersonAsync(userId.Value, dto.Nconst);

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

        var result = await userService.GetAllRatedTitlesAsync(
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

        var result = await userService.GetAllSearchHistoryAsync(
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

        var result = await userService.DeleteAllSearchHistoryAsync(userId.Value);

        if (!result)
            return NotFound("Search history is empty.");

        return NoContent();
    }
}
