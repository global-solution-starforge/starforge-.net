using Microsoft.EntityFrameworkCore;
using StarForge.Application.Interfaces;
using StarForge.Domain.Entities;
using StarForge.Infrastructure.Data;

namespace StarForge.Infrastructure.Repositories;

public class HangarRepository(StarForgeDbContext context) : RepositoryBase<Hangar>(context), IHangarRepository
{
    public async Task<IEnumerable<Hangar>> GetByUsuarioIdAsync(Guid usuarioId) =>
        await _context.Hangares.Where(h => h.UsuarioId == usuarioId).ToListAsync();

    public async Task<IEnumerable<Hangar>> GetByMissaoIdAsync(Guid missaoId) =>
        await _context.Hangares.Where(h => h.MissaoId == missaoId).ToListAsync();
}
