using StarForge.Domain.Entities;

namespace StarForge.Application.Interfaces;

/// <summary>
/// Repositório especializado para a entidade <see cref="FaseMissao"/>.
/// </summary>
public interface IFaseMissaoRepository : IRepositoryBase<FaseMissao>
{
    /// <summary>
    /// Retorna todas as fases de uma missão ordenadas pelo campo <c>Ordem</c>.
    /// </summary>
    /// <param name="missaoId">ID da missão.</param>
    /// <returns>Fases da missão em ordem crescente de exibição.</returns>
    Task<IEnumerable<FaseMissao>> GetByMissaoIdAsync(Guid missaoId);
}
