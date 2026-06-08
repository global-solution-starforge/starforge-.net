using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarForge.Application.DTOs.Tier;
using StarForge.Application.Interfaces.Services;

namespace StarForge.API.Controllers;

/// <summary>
/// Controller de tiers (níveis de recompensa) de missões.
/// Leitura é pública; criação e exclusão requerem role <c>Admin</c>.
/// </summary>
[ApiController]
[Route("api/tiers")]
public class TiersController(ITierService tierService) : ControllerBase
{
    /// <summary>
    /// Busca um tier pelo seu identificador único.
    /// </summary>
    /// <param name="id">GUID do tier.</param>
    /// <returns>Dados do tier incluindo valor mínimo e vagas disponíveis.</returns>
    /// <response code="200">Tier encontrado.</response>
    /// <response code="404">Tier não encontrado.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TierDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TierDto>> GetById(Guid id)
    {
        var result = await tierService.GetByIdAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Lista todos os tiers de uma missão específica.
    /// </summary>
    /// <param name="missaoId">GUID da missão proprietária dos tiers.</param>
    /// <returns>Coleção de tiers da missão ordenados por valor.</returns>
    /// <response code="200">Lista retornada com sucesso.</response>
    [HttpGet("missao/{missaoId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<TierDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TierDto>>> GetByMissao(Guid missaoId)
    {
        var result = await tierService.GetByMissaoIdAsync(missaoId);
        return Ok(result);
    }

    /// <summary>
    /// Cria um novo tier para uma missão.
    /// </summary>
    /// <param name="dto">Dados do tier: nome, valor, benefício e limite de vagas.</param>
    /// <returns>Tier criado com status 201 e Location header.</returns>
    /// <response code="201">Tier criado com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    /// <response code="401">Token JWT ausente ou inválido.</response>
    /// <response code="403">Usuário não possui role Admin.</response>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(TierDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<TierDto>> Criar([FromBody] CriarTierDto dto)
    {
        var result = await tierService.CriarAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Remove um tier do sistema.
    /// </summary>
    /// <param name="id">GUID do tier a remover.</param>
    /// <returns>204 No Content em caso de sucesso.</returns>
    /// <response code="204">Tier removido com sucesso.</response>
    /// <response code="401">Token JWT ausente ou inválido.</response>
    /// <response code="403">Usuário não possui role Admin.</response>
    /// <response code="404">Tier não encontrado.</response>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deletar(Guid id)
    {
        await tierService.DeletarAsync(id);
        return NoContent();
    }
}
