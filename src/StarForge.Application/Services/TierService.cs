using StarForge.Application.DTOs.Tier;
using StarForge.Application.Exceptions;
using StarForge.Application.Interfaces;
using StarForge.Application.Interfaces.Services;
using StarForge.Domain.Entities;

namespace StarForge.Application.Services;

/// <summary>
/// Implementação do serviço de gerenciamento de tiers (níveis de recompensa) de missões.
/// </summary>
/// <param name="tierRepo">Repositório de tiers.</param>
/// <param name="missaoRepo">Repositório de missões — usado para validar existência antes de criar.</param>
public class TierService(ITierRepository tierRepo, IMissaoRepository missaoRepo) : ITierService
{
    /// <inheritdoc />
    public async Task<TierDto> CriarAsync(CriarTierDto dto)
    {
        // Valida existência da missão antes de criar o tier — tier sem missão não faz sentido
        _ = await missaoRepo.GetByIdAsync(dto.MissaoId)
            ?? throw new NotFoundException(nameof(Missao), dto.MissaoId);

        var tier = new Tier(dto.Nome, dto.Valor, dto.BeneficioDescricao, dto.LimiteVagas, dto.MissaoId);
        await tierRepo.AddAsync(tier);
        await tierRepo.SaveChangesAsync();
        return MapToDto(tier);
    }

    /// <inheritdoc />
    public async Task<TierDto> GetByIdAsync(Guid id)
    {
        var tier = await tierRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Tier), id);
        return MapToDto(tier);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TierDto>> GetByMissaoIdAsync(Guid missaoId)
    {
        var tiers = await tierRepo.GetByMissaoIdAsync(missaoId);
        return tiers.Select(MapToDto);
    }

    /// <inheritdoc />
    public async Task DeletarAsync(Guid id)
    {
        var tier = await tierRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Tier), id);
        tierRepo.Delete(tier);
        await tierRepo.SaveChangesAsync();
    }

    /// <summary>Mapeia a entidade <see cref="Tier"/> para o DTO de resposta.</summary>
    private static TierDto MapToDto(Tier t) =>
        new(t.Id, t.Nome, t.Valor, t.BeneficioDescricao, t.LimiteVagas, t.VagasOcupadas, t.MissaoId);
}
