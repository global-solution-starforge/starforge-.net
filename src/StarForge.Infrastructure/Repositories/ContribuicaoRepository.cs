using Microsoft.EntityFrameworkCore;
using StarForge.Application.Interfaces;
using StarForge.Domain.Entities;
using StarForge.Infrastructure.Data;

namespace StarForge.Infrastructure.Repositories;

public class ContribuicaoRepository(StarForgeDbContext context) : RepositoryBase<Contribuicao>(context), IContribuicaoRepository
{
    public async Task<IEnumerable<Contribuicao>> GetByUsuarioIdAsync(Guid usuarioId) =>
        await _context.Contribuicoes.Where(c => c.UsuarioId == usuarioId).ToListAsync();

    public async Task<IEnumerable<Contribuicao>> GetByMissaoIdAsync(Guid missaoId) =>
        await _context.Contribuicoes.Where(c => c.MissaoId == missaoId).ToListAsync();
}
