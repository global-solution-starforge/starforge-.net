using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarForge.Application.DTOs.Contribuicao;
using StarForge.Application.Interfaces.Services;

namespace StarForge.API.Controllers;

/// <summary>
/// Controller do ciclo de vida das contribuições financeiras.
/// Todos os endpoints requerem autenticação JWT.
/// </summary>
/// <remarks>
/// Fluxo principal de uma contribuição:
/// <list type="number">
///   <item>POST /api/contribuicoes → cria com status <c>Pendente</c></item>
///   <item>PUT /api/contribuicoes/confirmar/{id} → confirma, libera nave no hangar, verifica meta</item>
///   <item>DELETE /api/contribuicoes/cancelar/{id} → cancela e remove hangar pendente associado</item>
/// </list>
/// </remarks>
[ApiController]
[Route("api/contribuicoes")]
[Authorize]
public class ContribuicoesController(IContribuicaoService contribuicaoService) : ControllerBase
{
    /// <summary>
    /// Registra uma nova contribuição para uma missão.
    /// </summary>
    /// <remarks>Cria a contribuição com status <c>Pendente</c> e reserva um hangar pendente para o usuário.</remarks>
    /// <param name="dto">Dados da contribuição: usuário, missão, tier, valor e método de pagamento.</param>
    /// <returns>Contribuição criada com status 201.</returns>
    /// <response code="201">Contribuição registrada com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    /// <response code="401">Token JWT ausente ou inválido.</response>
    /// <response code="422">Missão não ativa, tier sem vagas ou valor abaixo do mínimo do tier.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ContribuicaoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ContribuicaoDto>> Criar([FromBody] CriarContribuicaoDto dto)
    {
        var result = await contribuicaoService.CriarAsync(dto);
        return CreatedAtAction(nameof(GetByUsuario), new { usuarioId = result.UsuarioId }, result);
    }

    /// <summary>
    /// Confirma uma contribuição pendente, processando o pagamento.
    /// </summary>
    /// <remarks>
    /// Ao confirmar: atualiza o nível do usuário, registra arrecadação na missão,
    /// e verifica se a meta foi atingida (desbloqueando todos os hangares da missão nesse caso).
    /// </remarks>
    /// <param name="id">GUID da contribuição a confirmar.</param>
    /// <returns>Contribuição atualizada com status <c>Confirmada</c>.</returns>
    /// <response code="200">Contribuição confirmada com sucesso.</response>
    /// <response code="401">Token JWT ausente ou inválido.</response>
    /// <response code="404">Contribuição não encontrada.</response>
    /// <response code="422">Contribuição não está no status Pendente.</response>
    [HttpPut("confirmar/{id:guid}")]
    [ProducesResponseType(typeof(ContribuicaoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ContribuicaoDto>> Confirmar(Guid id)
    {
        var result = await contribuicaoService.ConfirmarAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Cancela uma contribuição pendente e remove o hangar reservado associado.
    /// </summary>
    /// <param name="id">GUID da contribuição a cancelar.</param>
    /// <returns>Contribuição atualizada com status <c>Cancelada</c>.</returns>
    /// <response code="200">Contribuição cancelada com sucesso.</response>
    /// <response code="401">Token JWT ausente ou inválido.</response>
    /// <response code="404">Contribuição não encontrada.</response>
    /// <response code="422">Contribuição não está no status Pendente.</response>
    [HttpDelete("cancelar/{id:guid}")]
    [ProducesResponseType(typeof(ContribuicaoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ContribuicaoDto>> Cancelar(Guid id)
    {
        var result = await contribuicaoService.CancelarAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Lista todas as contribuições de um usuário específico (por ID na rota).
    /// </summary>
    /// <param name="usuarioId">GUID do usuário cujas contribuições serão listadas.</param>
    /// <returns>Histórico de contribuições do usuário.</returns>
    /// <response code="200">Lista retornada com sucesso.</response>
    /// <response code="401">Token JWT ausente ou inválido.</response>
    [HttpGet("usuario/{usuarioId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<ContribuicaoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<ContribuicaoDto>>> GetByUsuario(Guid usuarioId)
    {
        var result = await contribuicaoService.GetByUsuarioIdAsync(usuarioId);
        return Ok(result);
    }

    /// <summary>
    /// Lista as contribuições do usuário autenticado (extraído do JWT).
    /// </summary>
    /// <remarks>Usa o claim <c>NameIdentifier</c> do token JWT para identificar o usuário.</remarks>
    /// <returns>Histórico de contribuições do usuário logado.</returns>
    /// <response code="200">Lista retornada com sucesso.</response>
    /// <response code="401">Token JWT ausente, inválido ou sem claim de usuário.</response>
    [HttpGet("minha")]
    [ProducesResponseType(typeof(IEnumerable<ContribuicaoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<ContribuicaoDto>>> GetMinhas()
    {
        // Extrai o UsuarioId do claim do JWT gerado pelo AuthService
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var usuarioId))
            return Unauthorized();

        var result = await contribuicaoService.GetByUsuarioIdAsync(usuarioId);
        return Ok(result);
    }
}
