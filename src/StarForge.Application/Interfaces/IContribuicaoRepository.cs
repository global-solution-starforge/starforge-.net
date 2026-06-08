using StarForge.Domain.Entities;

namespace StarForge.Application.Interfaces;

public interface IContribuicaoRepository : IRepositoryBase<Contribuicao>
{
    Task<IEnumerable<Contribuicao>> GetByUsuarioIdAsync(Guid usuarioId);
    Task<IEnumerable<Contribuicao>> GetByMissaoIdAsync(Guid missaoId);
}
