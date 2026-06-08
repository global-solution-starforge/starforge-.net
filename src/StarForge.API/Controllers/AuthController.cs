using Microsoft.AspNetCore.Mvc;
using StarForge.Application.DTOs.Auth;
using StarForge.Application.Interfaces.Services;

namespace StarForge.API.Controllers;

/// <summary>
/// Controller de autenticação.
/// Fornece o endpoint de login e geração de JWT para acesso à API.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>
    /// Autentica um usuário e retorna um token JWT.
    /// </summary>
    /// <param name="dto">Credenciais de acesso: email e senha.</param>
    /// <returns>Token JWT, data de expiração e dados básicos do usuário autenticado.</returns>
    /// <response code="200">Login bem-sucedido — retorna <see cref="TokenResponseDto"/>.</response>
    /// <response code="401">Credenciais inválidas (email não encontrado ou senha incorreta).</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenResponseDto>> Login([FromBody] LoginDto dto)
    {
        var result = await authService.LoginAsync(dto);
        return Ok(result);
    }
}
