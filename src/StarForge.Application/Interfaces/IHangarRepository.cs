using StarForge.Domain.Entities;

namespace StarForge.Application.Interfaces;

public interface IHangarRepository : IRepositoryBase<Hangar>
{
    Task<IEnumerable<Hangar>> GetByUsuarioIdAsync(Guid usuarioId);
    Task<IEnumerable<Hangar>> GetByMissaoIdAsync(Guid missaoId);
}
