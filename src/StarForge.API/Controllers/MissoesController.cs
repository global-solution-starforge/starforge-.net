using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarForge.Application.DTOs.Missao;
using StarForge.Application.Interfaces.Services;

namespace StarForge.API.Controllers;

/// <summary>
/// Controller de missões de crowdfunding espacial.
/// Leitura é pública; criação, atualização e exclusão requerem role <c>Admin</c>.
/// </summary>
[ApiController]
[Route("api/missoes")]
public class MissoesController(IMissaoService missaoService) : ControllerBase
{
    /// <summary>
    /// Lista todas as missões (de qualquer status).
    /// </summary>
    /// <returns>Coleção de todas as missões cadastradas.</returns>
    /// <response code="200">Lista retornada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MissaoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MissaoDto>>> GetAll()
    {
        var result = await missaoService.GetAllAsync();
        return Ok(result);
    }

    /// <summary>
    /// Lista apenas as missões com status <c>Ativa</c>.
    /// </summary>
    /// <returns>Missões abertas para contribuição.</returns>
    /// <response code="200">Lista retornada com sucesso.</response>
    [HttpGet("ativas")]
    [ProducesResponseType(typeof(IEnumerable<MissaoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MissaoDto>>> GetAtivas()
    {
        var result = await missaoService.GetAtivasAsync();
        return Ok(result);
    }

    /// <summary>
    /// Busca uma missão pelo seu identificador único.
    /// </summary>
    /// <param name="id">GUID da missão.</param>
    /// <returns>Dados completos da missão incluindo progresso de arrecadação.</returns>
    /// <response code="200">Missão encontrada.</response>
    /// <response code="404">Missão não encontrada.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(MissaoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MissaoDto>> GetById(Guid id)
    {
        var result = await missaoService.GetByIdAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Cria uma nova missão de crowdfunding.
    /// </summary>
    /// <param name="dto">Dados da missão: nome, descrição, meta financeira e datas.</param>
    /// <returns>Missão criada com status 201 e Location header.</returns>
    /// <response code="201">Missão criada com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    /// <response code="401">Token JWT ausente ou inválido.</response>
    /// <response code="403">Usuário não possui role Admin.</response>
    /// <response code="422">Regra de negócio violada (ex: DataInicio >= DataLimite).</response>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(MissaoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<MissaoDto>> Criar([FromBody] CriarMissaoDto dto)
    {
        var result = await missaoService.CriarAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Atualiza os dados de uma missão existente.
    /// </summary>
    /// <param name="id">GUID da missão a atualizar.</param>
    /// <param name="dto">Novos dados da missão.</param>
    /// <returns>Dados atualizados da missão.</returns>
    /// <response code="200">Missão atualizada com sucesso.</response>
    /// <response code="401">Token JWT ausente ou inválido.</response>
    /// <response code="403">Usuário não possui role Admin.</response>
    /// <response code="404">Missão não encontrada.</response>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(MissaoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MissaoDto>> Atualizar(Guid id, [FromBody] CriarMissaoDto dto)
    {
        var result = await missaoService.AtualizarAsync(id, dto);
        return Ok(result);
    }

    /// <summary>
    /// Remove uma missão do sistema.
    /// </summary>
    /// <param name="id">GUID da missão a remover.</param>
    /// <returns>204 No Content em caso de sucesso.</returns>
    /// <response code="204">Missão removida com sucesso.</response>
    /// <response code="401">Token JWT ausente ou inválido.</response>
    /// <response code="403">Usuário não possui role Admin.</response>
    /// <response code="404">Missão não encontrada.</response>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deletar(Guid id)
    {
        await missaoService.DeletarAsync(id);
        return NoContent();
    }
}
