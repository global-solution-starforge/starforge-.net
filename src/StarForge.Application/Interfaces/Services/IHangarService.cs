using StarForge.Application.DTOs.Hangar;

namespace StarForge.Application.Interfaces.Services;

public interface IHangarService
{
    Task<IEnumerable<HangarDto>> GetByUsuarioIdAsync(Guid usuarioId);
    Task<HangarDto> GetByIdAsync(Guid id);
}
