using StarForge.Application.DTOs.Nave;

namespace StarForge.Application.Interfaces.Services;

public interface INaveService
{
    Task<NaveDto> CriarAsync(CriarNaveDto dto);
    Task<NaveDto> GetByIdAsync(Guid id);
    Task<IEnumerable<NaveDto>> GetByMissaoIdAsync(Guid missaoId);
    Task DeletarAsync(Guid id);
}
