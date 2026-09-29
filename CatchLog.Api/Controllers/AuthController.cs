using CatchLog.Api.DTOs;
using CatchLog.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatchLog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDto>> Login(LoginDto dto)
    {
        var token = await _authService.LoginAsync(dto);
        if (token is null)
        {
            return Unauthorized(new { error = "Invalid email or password." });
        }

        return Ok(new TokenResponseDto { Token = token });
    }
}