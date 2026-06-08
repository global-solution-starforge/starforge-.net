using Microsoft.EntityFrameworkCore;
using StarForge.Application.Interfaces;
using StarForge.Domain.Entities;
using StarForge.Infrastructure.Data;

namespace StarForge.Infrastructure.Repositories;

/// <summary>
/// Repositório concreto para a entidade <see cref="Nave"/>.
/// Herda as operações CRUD de <see cref="RepositoryBase{T}"/> e adiciona
/// a consulta de naves disponíveis em uma missão.
/// </summary>
/// <param name="context">DbContext injetado pelo DI (Scoped por requisição).</param>
public class NaveRepository(StarForgeDbContext context) : RepositoryBase<Nave>(context), INaveRepository
{
    /// <inheritdoc />
    /// <remarks>
    /// Usado pelo <c>ContribuicaoService</c> ao confirmar uma contribuição: busca a nave
    /// da missão para associar ao hangar do usuário.
    /// Beneficia-se do índice <c>IX_TB_NAVE_MISSAO_ID</c> definido em <c>NaveConfiguration</c>.
    /// </remarks>
    public async Task<IEnumerable<Nave>> GetByMissaoIdAsync(Guid missaoId) =>
        await _context.Naves.Where(n => n.MissaoId == missaoId).ToListAsync();
}
