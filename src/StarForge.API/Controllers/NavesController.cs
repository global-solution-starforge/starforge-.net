using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarForge.Application.DTOs.Nave;
using StarForge.Application.Interfaces.Services;

namespace StarForge.API.Controllers;

[ApiController]
[Route("api/naves")]
public class NavesController(INaveService naveService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<NaveDto>> GetById(Guid id)
    {
        var result = await naveService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("missao/{missaoId:guid}")]
    public async Task<ActionResult<IEnumerable<NaveDto>>> GetByMissao(Guid missaoId)
    {
        var result = await naveService.GetByMissaoIdAsync(missaoId);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<NaveDto>> Criar([FromBody] CriarNaveDto dto)
    {
        var result = await naveService.CriarAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Deletar(Guid id)
    {
        await naveService.DeletarAsync(id);
        return NoContent();
    }
}
