using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarForge.Application.DTOs.FaseMissao;
using StarForge.Application.Interfaces.Services;

namespace StarForge.API.Controllers;

[ApiController]
[Route("api/fases-missao")]
public class FasesMissaoController(IFaseMissaoService faseMissaoService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FaseMissaoDto>> GetById(Guid id)
    {
        var result = await faseMissaoService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("missao/{missaoId:guid}")]
    public async Task<ActionResult<IEnumerable<FaseMissaoDto>>> GetByMissao(Guid missaoId)
    {
        var result = await faseMissaoService.GetByMissaoIdAsync(missaoId);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<FaseMissaoDto>> Criar([FromBody] CriarFaseMissaoDto dto)
    {
        var result = await faseMissaoService.CriarAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<FaseMissaoDto>> Atualizar(Guid id, [FromBody] AtualizarFaseMissaoDto dto)
    {
        var result = await faseMissaoService.AtualizarAsync(id, dto);
        return Ok(result);
    }

    [HttpPut("concluir/{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<FaseMissaoDto>> Concluir(Guid id)
    {
        var result = await faseMissaoService.ConcluirAsync(id);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Deletar(Guid id)
    {
        await faseMissaoService.DeletarAsync(id);
        return NoContent();
    }
}
