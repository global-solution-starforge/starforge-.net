using Microsoft.EntityFrameworkCore;
using StarForge.Application.Interfaces;
using StarForge.Domain.Entities;
using StarForge.Infrastructure.Data;

namespace StarForge.Infrastructure.Repositories;

/// <summary>
/// Repositório concreto para a entidade <see cref="Hangar"/>.
/// Herda as operações CRUD de <see cref="RepositoryBase{T}"/> e adiciona
/// consultas por usuário e por missão.
/// </summary>
/// <param name="context">DbContext injetado pelo DI (Scoped por requisição).</param>
public class HangarRepository(StarForgeDbContext context) : RepositoryBase<Hangar>(context), IHangarRepository
{
    /// <inheritdoc />
    /// <remarks>
    /// Usado em <c>GET /api/hangar</c> para retornar todas as naves (pendentes e desbloqueadas)
    /// do usuário autenticado. Também usado em <c>ContribuicaoService.CancelarAsync</c>
    /// para localizar e remover o hangar pendente órfão quando uma contribuição é cancelada.
    /// </remarks>
    public async Task<IEnumerable<Hangar>> GetByUsuarioIdAsync(Guid usuarioId) =>
        await _context.Hangares.Where(h => h.UsuarioId == usuarioId).ToListAsync();

    /// <inheritdoc />
    /// <remarks>
    /// Usado em <c>ContribuicaoService.ConfirmarAsync</c> para desbloquear todos os hangares
    /// pendentes de uma missão quando ela atinge a meta de financiamento.
    /// </remarks>
    public async Task<IEnumerable<Hangar>> GetByMissaoIdAsync(Guid missaoId) =>
        await _context.Hangares.Where(h => h.MissaoId == missaoId).ToListAsync();
}
