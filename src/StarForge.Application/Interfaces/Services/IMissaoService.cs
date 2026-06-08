using StarForge.Application.DTOs.Missao;

namespace StarForge.Application.Interfaces.Services;

public interface IMissaoService
{
    Task<MissaoDto> CriarAsync(CriarMissaoDto dto);
    Task<MissaoDto> GetByIdAsync(Guid id);
    Task<IEnumerable<MissaoDto>> GetAllAsync();
    Task<IEnumerable<MissaoDto>> GetAtivasAsync();
    Task<MissaoDto> AtualizarAsync(Guid id, CriarMissaoDto dto);
    Task DeletarAsync(Guid id);
}
