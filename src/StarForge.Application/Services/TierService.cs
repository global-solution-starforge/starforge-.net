using StarForge.Application.DTOs.Tier;
using StarForge.Application.Exceptions;
using StarForge.Application.Interfaces;
using StarForge.Application.Interfaces.Services;
using StarForge.Domain.Entities;

namespace StarForge.Application.Services;

public class TierService(ITierRepository tierRepo, IMissaoRepository missaoRepo) : ITierService
{
    public async Task<TierDto> CriarAsync(CriarTierDto dto)
    {
        _ = await missaoRepo.GetByIdAsync(dto.MissaoId)
            ?? throw new NotFoundException(nameof(Missao), dto.MissaoId);

        var tier = new Tier(dto.Nome, dto.Valor, dto.BeneficioDescricao, dto.LimiteVagas, dto.MissaoId);
        await tierRepo.AddAsync(tier);
        await tierRepo.SaveChangesAsync();
        return MapToDto(tier);
    }

    public async Task<TierDto> GetByIdAsync(Guid id)
    {
        var tier = await tierRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Tier), id);
        return MapToDto(tier);
    }

    public async Task<IEnumerable<TierDto>> GetByMissaoIdAsync(Guid missaoId)
    {
        var tiers = await tierRepo.GetByMissaoIdAsync(missaoId);
        return tiers.Select(MapToDto);
    }

    public async Task DeletarAsync(Guid id)
    {
        var tier = await tierRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Tier), id);
        tierRepo.Delete(tier);
        await tierRepo.SaveChangesAsync();
    }

    private static TierDto MapToDto(Tier t) =>
        new(t.Id, t.Nome, t.Valor, t.BeneficioDescricao, t.LimiteVagas, t.VagasOcupadas, t.MissaoId);
}
