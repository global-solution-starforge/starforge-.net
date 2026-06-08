using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarForge.Application.DTOs.Usuario;
using StarForge.Application.Interfaces.Services;

namespace StarForge.API.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsuariosController(IUsuarioService usuarioService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<UsuarioDto>> Criar([FromBody] CriarUsuarioDto dto)
    {
        var result = await usuarioService.CriarAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetAll()
    {
        var result = await usuarioService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<UsuarioDto>> GetById(Guid id)
    {
        var result = await usuarioService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<UsuarioDto>> Atualizar(Guid id, [FromBody] AtualizarUsuarioDto dto)
    {
        var result = await usuarioService.AtualizarAsync(id, dto);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Desativar(Guid id)
    {
        await usuarioService.DesativarAsync(id);
        return NoContent();
    }
}
