using StarForge.Domain.Entities;

namespace StarForge.Application.Interfaces;

public interface IFaseMissaoRepository : IRepositoryBase<FaseMissao>
{
    Task<IEnumerable<FaseMissao>> GetByMissaoIdAsync(Guid missaoId);
}
