using Microsoft.EntityFrameworkCore;
using StarForge.Application.Interfaces;
using StarForge.Domain.Entities;
using StarForge.Infrastructure.Data;

namespace StarForge.Infrastructure.Repositories;

/// <summary>
/// Repositório concreto para a entidade <see cref="Contribuicao"/>.
/// Herda as operações CRUD de <see cref="RepositoryBase{T}"/> e adiciona
/// consultas por usuário e por missão.
/// </summary>
/// <param name="context">DbContext injetado pelo DI (Scoped por requisição).</param>
public class ContribuicaoRepository(StarForgeDbContext context) : RepositoryBase<Contribuicao>(context), IContribuicaoRepository
{
    /// <inheritdoc />
    /// <remarks>
    /// Usado para exibir o histórico de contribuições do usuário autenticado.
    /// </remarks>
    public async Task<IEnumerable<Contribuicao>> GetByUsuarioIdAsync(Guid usuarioId) =>
        await _context.Contribuicoes.Where(c => c.UsuarioId == usuarioId).ToListAsync();

    /// <inheritdoc />
    /// <remarks>
    /// Usado em <c>ContribuicaoService.ConfirmarAsync</c> para localizar todas as contribuições
    /// pendentes de uma missão quando ela falha, a fim de marcá-las para reembolso.
    /// </remarks>
    public async Task<IEnumerable<Contribuicao>> GetByMissaoIdAsync(Guid missaoId) =>
        await _context.Contribuicoes.Where(c => c.MissaoId == missaoId).ToListAsync();
}
