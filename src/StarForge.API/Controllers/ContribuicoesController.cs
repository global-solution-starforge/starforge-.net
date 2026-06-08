using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarForge.Application.DTOs.Contribuicao;
using StarForge.Application.Interfaces.Services;

namespace StarForge.API.Controllers;

[ApiController]
[Route("api/contribuicoes")]
[Authorize]
public class ContribuicoesController(IContribuicaoService contribuicaoService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ContribuicaoDto>> Criar([FromBody] CriarContribuicaoDto dto)
    {
        var result = await contribuicaoService.CriarAsync(dto);
        return CreatedAtAction(nameof(GetByUsuario), new { usuarioId = result.UsuarioId }, result);
    }

    [HttpPut("confirmar/{id:guid}")]
    public async Task<ActionResult<ContribuicaoDto>> Confirmar(Guid id)
    {
        var result = await contribuicaoService.ConfirmarAsync(id);
        return Ok(result);
    }

    [HttpDelete("cancelar/{id:guid}")]
    public async Task<ActionResult<ContribuicaoDto>> Cancelar(Guid id)
    {
        var result = await contribuicaoService.CancelarAsync(id);
        return Ok(result);
    }

    [HttpGet("usuario/{usuarioId:guid}")]
    public async Task<ActionResult<IEnumerable<ContribuicaoDto>>> GetByUsuario(Guid usuarioId)
    {
        var result = await contribuicaoService.GetByUsuarioIdAsync(usuarioId);
        return Ok(result);
    }

    [HttpGet("minha")]
    public async Task<ActionResult<IEnumerable<ContribuicaoDto>>> GetMinhas()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var usuarioId))
            return Unauthorized();

        var result = await contribuicaoService.GetByUsuarioIdAsync(usuarioId);
        return Ok(result);
    }
}
