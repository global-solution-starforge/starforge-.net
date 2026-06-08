using StarForge.Domain.Entities;
using StarForge.Domain.Enums;

namespace StarForge.Application.Interfaces;

public interface IMissaoRepository : IRepositoryBase<Missao>
{
    Task<IEnumerable<Missao>> GetByStatusAsync(StatusMissao status);
}
