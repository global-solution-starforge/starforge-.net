using Microsoft.EntityFrameworkCore;
using StarForge.Application.Interfaces;
using StarForge.Domain.Entities;
using StarForge.Infrastructure.Data;

namespace StarForge.Infrastructure.Repositories;

public class FaseMissaoRepository(StarForgeDbContext context) : RepositoryBase<FaseMissao>(context), IFaseMissaoRepository
{
    public async Task<IEnumerable<FaseMissao>> GetByMissaoIdAsync(Guid missaoId) =>
        await _context.FasesMissao.Where(f => f.MissaoId == missaoId).ToListAsync();
}
