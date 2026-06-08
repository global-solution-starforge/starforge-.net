using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarForge.Application.DTOs.Hangar;
using StarForge.Application.Interfaces.Services;

namespace StarForge.API.Controllers;

/// <summary>
/// Controller do hangar pessoal de naves do usuário.
/// Exibe as naves desbloqueadas (missão concluída) e pendentes (aguardando conclusão).
/// Todos os endpoints requerem autenticação JWT.
/// </summary>
[ApiController]
[Route("api/hangar")]
[Authorize]
public class HangarController(IHangarService hangarService) : ControllerBase
{
    /// <summary>
    /// Retorna o hangar completo do usuário autenticado.
    /// </summary>
    /// <remarks>
    /// O hangar inclui naves com status <c>Pendente</c> (missão ainda ativa) e
    /// <c>Desbloqueada</c> (missão concluída com meta atingida).
    /// Usa o claim <c>NameIdentifier</c> do JWT para identificar o usuário.
    /// </remarks>
    /// <returns>Coleção de entradas do hangar com dados da nave associada.</returns>
    /// <response code="200">Hangar retornado com sucesso.</response>
    /// <response code="401">Token JWT ausente, inválido ou sem claim de usuário.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<HangarDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<HangarDto>>> GetMeuHangar()
    {
        // Extrai o UsuarioId diretamente do token JWT — evita passar o ID na rota (segurança)
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var usuarioId))
            return Unauthorized();

        var result = await hangarService.GetByUsuarioIdAsync(usuarioId);
        return Ok(result);
    }

    /// <summary>
    /// Busca uma entrada específica do hangar pelo seu identificador.
    /// </summary>
    /// <param name="id">GUID da entrada do hangar.</param>
    /// <returns>Dados da entrada do hangar com informações da nave.</returns>
    /// <response code="200">Entrada encontrada.</response>
    /// <response code="401">Token JWT ausente ou inválido.</response>
    /// <response code="404">Entrada do hangar não encontrada.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(HangarDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<HangarDto>> GetById(Guid id)
    {
        var result = await hangarService.GetByIdAsync(id);
        return Ok(result);
    }
}
