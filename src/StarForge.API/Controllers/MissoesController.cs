using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarForge.Application.DTOs.Missao;
using StarForge.Application.Interfaces.Services;

namespace StarForge.API.Controllers;

[ApiController]
[Route("api/missoes")]
public class MissoesController(IMissaoService missaoService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MissaoDto>>> GetAll()
    {
        var result = await missaoService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("ativas")]
    public async Task<ActionResult<IEnumerable<MissaoDto>>> GetAtivas()
    {
        var result = await missaoService.GetAtivasAsync();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MissaoDto>> GetById(Guid id)
    {
        var result = await missaoService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<MissaoDto>> Criar([FromBody] CriarMissaoDto dto)
    {
        var result = await missaoService.CriarAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<MissaoDto>> Atualizar(Guid id, [FromBody] CriarMissaoDto dto)
    {
        var result = await missaoService.AtualizarAsync(id, dto);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Deletar(Guid id)
    {
        await missaoService.DeletarAsync(id);
        return NoContent();
    }
}
