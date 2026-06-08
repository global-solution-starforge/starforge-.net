using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarForge.Application.DTOs.Hangar;
using StarForge.Application.Interfaces.Services;

namespace StarForge.API.Controllers;

[ApiController]
[Route("api/hangar")]
[Authorize]
public class HangarController(IHangarService hangarService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HangarDto>>> GetMeuHangar()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var usuarioId))
            return Unauthorized();

        var result = await hangarService.GetByUsuarioIdAsync(usuarioId);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<HangarDto>> GetById(Guid id)
    {
        var result = await hangarService.GetByIdAsync(id);
        return Ok(result);
    }
}
