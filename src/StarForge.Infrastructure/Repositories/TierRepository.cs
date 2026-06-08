using Microsoft.EntityFrameworkCore;
using StarForge.Application.Interfaces;
using StarForge.Domain.Entities;
using StarForge.Infrastructure.Data;

namespace StarForge.Infrastructure.Repositories;

/// <summary>
/// Repositório concreto para a entidade <see cref="Tier"/>.
/// Herda as operações CRUD de <see cref="RepositoryBase{T}"/> e adiciona
/// a consulta de tiers de uma missão específica.
/// </summary>
/// <param name="context">DbContext injetado pelo DI (Scoped por requisição).</param>
public class TierRepository(StarForgeDbContext context) : RepositoryBase<Tier>(context), ITierRepository
{
    /// <inheritdoc />
    /// <remarks>
    /// Beneficia-se do índice <c>IX_TB_TIER_MISSAO_ID</c> definido em
    /// <c>TierConfiguration</c> para evitar full scan da tabela TB_TIER.
    /// </remarks>
    public async Task<IEnumerable<Tier>> GetByMissaoIdAsync(Guid missaoId) =>
        await _context.Tiers.Where(t => t.MissaoId == missaoId).ToListAsync();
}
