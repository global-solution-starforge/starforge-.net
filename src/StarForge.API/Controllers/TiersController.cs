using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarForge.Application.DTOs.Tier;
using StarForge.Application.Interfaces.Services;

namespace StarForge.API.Controllers;

[ApiController]
[Route("api/tiers")]
public class TiersController(ITierService tierService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TierDto>> GetById(Guid id)
    {
        var result = await tierService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("missao/{missaoId:guid}")]
    public async Task<ActionResult<IEnumerable<TierDto>>> GetByMissao(Guid missaoId)
    {
        var result = await tierService.GetByMissaoIdAsync(missaoId);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<TierDto>> Criar([FromBody] CriarTierDto dto)
    {
        var result = await tierService.CriarAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Deletar(Guid id)
    {
        await tierService.DeletarAsync(id);
        return NoContent();
    }
}
