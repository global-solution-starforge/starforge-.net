using StarForge.Domain.Entities;

namespace StarForge.Application.Interfaces;

public interface INaveRepository : IRepositoryBase<Nave>
{
    Task<IEnumerable<Nave>> GetByMissaoIdAsync(Guid missaoId);
}
