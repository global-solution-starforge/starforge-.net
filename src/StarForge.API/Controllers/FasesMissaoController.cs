using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarForge.Application.DTOs.FaseMissao;
using StarForge.Application.Interfaces.Services;

namespace StarForge.API.Controllers;

/// <summary>
/// Controller das fases narrativas das missões.
/// Leitura é pública; gerenciamento (criação, atualização, conclusão, exclusão) requer role <c>Admin</c>.
/// </summary>
[ApiController]
[Route("api/fases-missao")]
public class FasesMissaoController(IFaseMissaoService faseMissaoService) : ControllerBase
{
    /// <summary>
    /// Busca uma fase de missão pelo seu identificador único.
    /// </summary>
    /// <param name="id">GUID da fase.</param>
    /// <returns>Dados completos da fase incluindo status e data de conclusão.</returns>
    /// <response code="200">Fase encontrada.</response>
    /// <response code="404">Fase não encontrada.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(FaseMissaoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FaseMissaoDto>> GetById(Guid id)
    {
        var result = await faseMissaoService.GetByIdAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Lista todas as fases de uma missão específica.
    /// </summary>
    /// <param name="missaoId">GUID da missão proprietária das fases.</param>
    /// <returns>Coleção de fases da missão (ordenação por campo <c>Ordem</c> recomendada no cliente).</returns>
    /// <response code="200">Lista retornada com sucesso.</response>
    [HttpGet("missao/{missaoId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<FaseMissaoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<FaseMissaoDto>>> GetByMissao(Guid missaoId)
    {
        var result = await faseMissaoService.GetByMissaoIdAsync(missaoId);
        return Ok(result);
    }

    /// <summary>
    /// Cria uma nova fase narrativa para uma missão.
    /// </summary>
    /// <param name="dto">Dados da fase: missão, título, descrição e ordem de exibição.</param>
    /// <returns>Fase criada com status 201 e Location header.</returns>
    /// <response code="201">Fase criada com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    /// <response code="401">Token JWT ausente ou inválido.</response>
    /// <response code="403">Usuário não possui role Admin.</response>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(FaseMissaoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<FaseMissaoDto>> Criar([FromBody] CriarFaseMissaoDto dto)
    {
        var result = await faseMissaoService.CriarAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Atualiza os dados editáveis de uma fase (título, descrição e ordem).
    /// </summary>
    /// <param name="id">GUID da fase a atualizar.</param>
    /// <param name="dto">Novos dados da fase.</param>
    /// <returns>Fase atualizada.</returns>
    /// <response code="200">Fase atualizada com sucesso.</response>
    /// <response code="401">Token JWT ausente ou inválido.</response>
    /// <response code="403">Usuário não possui role Admin.</response>
    /// <response code="404">Fase não encontrada.</response>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(FaseMissaoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FaseMissaoDto>> Atualizar(Guid id, [FromBody] AtualizarFaseMissaoDto dto)
    {
        var result = await faseMissaoService.AtualizarAsync(id, dto);
        return Ok(result);
    }

    /// <summary>
    /// Marca uma fase como concluída, registrando a data e hora (UTC) de conclusão.
    /// </summary>
    /// <remarks>Esta operação é irreversível — uma fase concluída não pode voltar a status anterior.</remarks>
    /// <param name="id">GUID da fase a concluir.</param>
    /// <returns>Fase atualizada com status <c>Concluida</c> e <c>DataConclusao</c> preenchida.</returns>
    /// <response code="200">Fase concluída com sucesso.</response>
    /// <response code="401">Token JWT ausente ou inválido.</response>
    /// <response code="403">Usuário não possui role Admin.</response>
    /// <response code="404">Fase não encontrada.</response>
    /// <response code="422">Fase já concluída.</response>
    [HttpPut("concluir/{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(FaseMissaoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<FaseMissaoDto>> Concluir(Guid id)
    {
        var result = await faseMissaoService.ConcluirAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Remove uma fase narrativa do sistema.
    /// </summary>
    /// <param name="id">GUID da fase a remover.</param>
    /// <returns>204 No Content em caso de sucesso.</returns>
    /// <response code="204">Fase removida com sucesso.</response>
    /// <response code="401">Token JWT ausente ou inválido.</response>
    /// <response code="403">Usuário não possui role Admin.</response>
    /// <response code="404">Fase não encontrada.</response>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deletar(Guid id)
    {
        await faseMissaoService.DeletarAsync(id);
        return NoContent();
    }
}
