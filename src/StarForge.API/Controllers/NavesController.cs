using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarForge.Application.DTOs.Nave;
using StarForge.Application.Interfaces.Services;

namespace StarForge.API.Controllers;

/// <summary>
/// Controller de naves espaciais disponíveis como recompensas nas missões.
/// Leitura é pública; criação e exclusão requerem role <c>Admin</c>.
/// </summary>
[ApiController]
[Route("api/naves")]
public class NavesController(INaveService naveService) : ControllerBase
{
    /// <summary>
    /// Busca uma nave pelo seu identificador único.
    /// </summary>
    /// <param name="id">GUID da nave.</param>
    /// <returns>Dados da nave incluindo modelo, raridade e URL da imagem.</returns>
    /// <response code="200">Nave encontrada.</response>
    /// <response code="404">Nave não encontrada.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(NaveDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NaveDto>> GetById(Guid id)
    {
        var result = await naveService.GetByIdAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Lista todas as naves disponíveis em uma missão específica.
    /// </summary>
    /// <param name="missaoId">GUID da missão proprietária das naves.</param>
    /// <returns>Coleção de naves da missão.</returns>
    /// <response code="200">Lista retornada com sucesso.</response>
    [HttpGet("missao/{missaoId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<NaveDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<NaveDto>>> GetByMissao(Guid missaoId)
    {
        var result = await naveService.GetByMissaoIdAsync(missaoId);
        return Ok(result);
    }

    /// <summary>
    /// Cadastra uma nova nave como recompensa de uma missão.
    /// </summary>
    /// <param name="dto">Dados da nave: nome, modelo, descrição, raridade e missão.</param>
    /// <returns>Nave criada com status 201 e Location header.</returns>
    /// <response code="201">Nave criada com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    /// <response code="401">Token JWT ausente ou inválido.</response>
    /// <response code="403">Usuário não possui role Admin.</response>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(NaveDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<NaveDto>> Criar([FromBody] CriarNaveDto dto)
    {
        var result = await naveService.CriarAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Remove uma nave do sistema.
    /// </summary>
    /// <param name="id">GUID da nave a remover.</param>
    /// <returns>204 No Content em caso de sucesso.</returns>
    /// <response code="204">Nave removida com sucesso.</response>
    /// <response code="401">Token JWT ausente ou inválido.</response>
    /// <response code="403">Usuário não possui role Admin.</response>
    /// <response code="404">Nave não encontrada.</response>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deletar(Guid id)
    {
        await naveService.DeletarAsync(id);
        return NoContent();
    }
}
