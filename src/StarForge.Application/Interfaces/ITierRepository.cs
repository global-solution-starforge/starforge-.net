using StarForge.Domain.Entities;

namespace StarForge.Application.Interfaces;

public interface ITierRepository : IRepositoryBase<Tier>
{
    Task<IEnumerable<Tier>> GetByMissaoIdAsync(Guid missaoId);
}
