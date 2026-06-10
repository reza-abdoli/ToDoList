using Application.Enum;
using Application.Interface;
using Data.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public UserController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] SignupLoginDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _userService.Signup(dto);

        return result switch
        {
            ServiceResult.Success => Ok(new { message = "User created successfully" }),
            ServiceResult.AlreadyExists => BadRequest(new { message = "User already exists" }),
            _ => BadRequest()
        };
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] SignupLoginDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userService.Login(dto);
        if (user == null)
            return Unauthorized(new { message = "Invalid credentials." });

        var token = _tokenService.GenerateToken(user);
        return Ok(new { message = "logged in", token });
    }
}
