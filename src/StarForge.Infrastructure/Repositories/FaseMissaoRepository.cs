using Microsoft.EntityFrameworkCore;
using StarForge.Application.Interfaces;
using StarForge.Domain.Entities;
using StarForge.Infrastructure.Data;

namespace StarForge.Infrastructure.Repositories;

/// <summary>
/// Repositório concreto para a entidade <see cref="FaseMissao"/>.
/// Herda as operações CRUD de <see cref="RepositoryBase{T}"/> e adiciona
/// a consulta de fases de uma missão específica.
/// </summary>
/// <param name="context">DbContext injetado pelo DI (Scoped por requisição).</param>
public class FaseMissaoRepository(StarForgeDbContext context) : RepositoryBase<FaseMissao>(context), IFaseMissaoRepository
{
    /// <inheritdoc />
    /// <remarks>
    /// Retorna as fases já ordenadas pelo campo <c>Ordem</c>? Não — a ordenação
    /// deve ser aplicada pela camada de serviço/controller conforme necessário,
    /// pois diferentes endpoints podem precisar de diferentes ordenações.
    /// Beneficia-se do índice <c>IX_TB_FASE_MISSAO_MISSAO_ID</c> definido em
    /// <c>FaseMissaoConfiguration</c>.
    /// </remarks>
    public async Task<IEnumerable<FaseMissao>> GetByMissaoIdAsync(Guid missaoId) =>
        await _context.FasesMissao.Where(f => f.MissaoId == missaoId).ToListAsync();
}
