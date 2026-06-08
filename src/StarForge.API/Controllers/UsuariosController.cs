using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarForge.Application.DTOs.Usuario;
using StarForge.Application.Interfaces.Services;

namespace StarForge.API.Controllers;

/// <summary>
/// Controller de gerenciamento de usuários/pilotos.
/// O registro é público (self-service); demais operações requerem autenticação JWT.
/// </summary>
[ApiController]
[Route("api/usuarios")]
public class UsuariosController(IUsuarioService usuarioService) : ControllerBase
{
    /// <summary>
    /// Cria um novo usuário/piloto no sistema.
    /// </summary>
    /// <remarks>Este endpoint é público — não requer token JWT.</remarks>
    /// <param name="dto">Dados de cadastro: nome, email, RM e senha.</param>
    /// <returns>Dados do usuário recém-criado com status 201 e Location header.</returns>
    /// <response code="201">Usuário criado com sucesso.</response>
    /// <response code="400">Dados inválidos (campos obrigatórios ausentes ou formato incorreto).</response>
    /// <response code="422">Email ou RM já cadastrado.</response>
    [HttpPost]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<UsuarioDto>> Criar([FromBody] CriarUsuarioDto dto)
    {
        var result = await usuarioService.CriarAsync(dto);
        // CreatedAtAction define o Location header apontando para GET /api/usuarios/{id}
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Lista todos os usuários cadastrados.
    /// </summary>
    /// <returns>Coleção de usuários (sem dados sensíveis como SenhaHash).</returns>
    /// <response code="200">Lista retornada com sucesso.</response>
    /// <response code="401">Token JWT ausente ou inválido.</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<UsuarioDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetAll()
    {
        var result = await usuarioService.GetAllAsync();
        return Ok(result);
    }

    /// <summary>
    /// Busca um usuário pelo seu identificador único.
    /// </summary>
    /// <param name="id">GUID do usuário.</param>
    /// <returns>Dados do usuário encontrado.</returns>
    /// <response code="200">Usuário encontrado.</response>
    /// <response code="401">Token JWT ausente ou inválido.</response>
    /// <response code="404">Usuário não encontrado.</response>
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UsuarioDto>> GetById(Guid id)
    {
        var result = await usuarioService.GetByIdAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Atualiza os dados de um usuário existente.
    /// </summary>
    /// <param name="id">GUID do usuário a atualizar.</param>
    /// <param name="dto">Novos dados do usuário.</param>
    /// <returns>Dados atualizados do usuário.</returns>
    /// <response code="200">Usuário atualizado com sucesso.</response>
    /// <response code="401">Token JWT ausente ou inválido.</response>
    /// <response code="404">Usuário não encontrado.</response>
    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UsuarioDto>> Atualizar(Guid id, [FromBody] AtualizarUsuarioDto dto)
    {
        var result = await usuarioService.AtualizarAsync(id, dto);
        return Ok(result);
    }

    /// <summary>
    /// Desativa (soft delete) um usuário — não remove do banco.
    /// </summary>
    /// <param name="id">GUID do usuário a desativar.</param>
    /// <returns>204 No Content em caso de sucesso.</returns>
    /// <response code="204">Usuário desativado com sucesso.</response>
    /// <response code="401">Token JWT ausente ou inválido.</response>
    /// <response code="404">Usuário não encontrado.</response>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Desativar(Guid id)
    {
        await usuarioService.DesativarAsync(id);
        return NoContent();
    }
}
