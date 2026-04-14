using Microsoft.AspNetCore.Mvc;
using MiniLedger.Api.DTOs.Auth;
using MiniLedger.Api.Services.Interfaces;

namespace MiniLedger.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto dto)
    {
        var message = await _authService.RegisterAsync(dto);
        return Ok(new { message });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        return Ok(result);
    }
}