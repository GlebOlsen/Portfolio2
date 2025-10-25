using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ImdbClone.Api.DTOs.Users;
using ImdbClone.Api.Interfaces;
using ImdbClone.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ImdbClone.Api.Controllers;

[ApiController]
[Route("users")]
public class UsersController(IUserService userService, Hashing hashing, IConfiguration configuration) : ControllerBase
{
    [HttpPost]
    public IActionResult SignUp(CreateUserDto dto)
    {
        if (userService.GetUser(dto.Username) is not null || string.IsNullOrEmpty(dto.Password)) return BadRequest();
        
        var (hashedPwd, salt) = hashing.Hash(dto.Password);

        userService.CreateUser(dto.Name, dto.Username, dto.Email, hashedPwd, salt);

        return Ok();
    }
    
    [HttpPost("login")]
    public IActionResult Login(LoginUserDto dto)
    {
        var user = userService.GetUser(dto.Username);

        if(user == null)
        {
            return BadRequest();
        }

        if(!hashing.Verify(dto.Password, user.PasswordHash, user.Salt))
        {
            return BadRequest();
        }

        var claims = new List<Claim>
        {
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
}