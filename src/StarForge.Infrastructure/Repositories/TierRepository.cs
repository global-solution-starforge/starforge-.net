using Microsoft.EntityFrameworkCore;
using StarForge.Application.Interfaces;
using StarForge.Domain.Entities;
using StarForge.Infrastructure.Data;

namespace StarForge.Infrastructure.Repositories;

public class TierRepository(StarForgeDbContext context) : RepositoryBase<Tier>(context), ITierRepository
{
    public async Task<IEnumerable<Tier>> GetByMissaoIdAsync(Guid missaoId) =>
        await _context.Tiers.Where(t => t.MissaoId == missaoId).ToListAsync();
}
