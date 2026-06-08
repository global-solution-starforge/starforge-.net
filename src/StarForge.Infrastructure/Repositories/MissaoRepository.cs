using Microsoft.EntityFrameworkCore;
using StarForge.Application.Interfaces;
using StarForge.Domain.Entities;
using StarForge.Domain.Enums;
using StarForge.Infrastructure.Data;

namespace StarForge.Infrastructure.Repositories;

/// <summary>
/// Repositório concreto para a entidade <see cref="Missao"/>.
/// Herda as operações CRUD de <see cref="RepositoryBase{T}"/> e adiciona
/// a consulta por status de missão.
/// </summary>
/// <param name="context">DbContext injetado pelo DI (Scoped por requisição).</param>
public class MissaoRepository(StarForgeDbContext context) : RepositoryBase<Missao>(context), IMissaoRepository
{
    /// <inheritdoc />
    /// <remarks>
    /// Filtra missões por status diretamente no banco Oracle (predicado traduzido para SQL pelo EF Core).
    /// Usos comuns: buscar missões <c>Ativa</c> para exibição na listagem pública;
    /// buscar missões <c>Concluida</c> ou <c>Falhou</c> para relatórios.
    /// </remarks>
    public async Task<IEnumerable<Missao>> GetByStatusAsync(StatusMissao status) =>
        await _context.Missoes.Where(m => m.Status == status).ToListAsync();
}
