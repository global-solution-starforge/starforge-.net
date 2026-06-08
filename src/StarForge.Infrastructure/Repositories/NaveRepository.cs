using Microsoft.EntityFrameworkCore;
using StarForge.Application.Interfaces;
using StarForge.Domain.Entities;
using StarForge.Infrastructure.Data;

namespace StarForge.Infrastructure.Repositories;

public class NaveRepository(StarForgeDbContext context) : RepositoryBase<Nave>(context), INaveRepository
{
    public async Task<IEnumerable<Nave>> GetByMissaoIdAsync(Guid missaoId) =>
        await _context.Naves.Where(n => n.MissaoId == missaoId).ToListAsync();
}
