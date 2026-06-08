using Microsoft.EntityFrameworkCore;
using StarForge.Application.Interfaces;
using StarForge.Domain.Entities;
using StarForge.Domain.Enums;
using StarForge.Infrastructure.Data;

namespace StarForge.Infrastructure.Repositories;

public class MissaoRepository(StarForgeDbContext context) : RepositoryBase<Missao>(context), IMissaoRepository
{
    public async Task<IEnumerable<Missao>> GetByStatusAsync(StatusMissao status) =>
        await _context.Missoes.Where(m => m.Status == status).ToListAsync();
}
