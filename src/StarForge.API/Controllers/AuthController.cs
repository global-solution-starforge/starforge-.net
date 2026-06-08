using Microsoft.AspNetCore.Mvc;
using StarForge.Application.DTOs.Auth;
using StarForge.Application.Interfaces.Services;

namespace StarForge.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDto>> Login([FromBody] LoginDto dto)
    {
        var result = await authService.LoginAsync(dto);
        return Ok(result);
    }
}
