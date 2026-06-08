using StarForge.Application.DTOs.Tier;

namespace StarForge.Application.Interfaces.Services;

public interface ITierService
{
    Task<TierDto> CriarAsync(CriarTierDto dto);
    Task<TierDto> GetByIdAsync(Guid id);
    Task<IEnumerable<TierDto>> GetByMissaoIdAsync(Guid missaoId);
    Task DeletarAsync(Guid id);
}
