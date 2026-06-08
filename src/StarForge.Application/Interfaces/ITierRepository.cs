using StarForge.Domain.Entities;

namespace StarForge.Application.Interfaces;

/// <summary>
/// Repositório especializado para a entidade <see cref="Tier"/>.
/// </summary>
public interface ITierRepository : IRepositoryBase<Tier>
{
    /// <summary>
    /// Retorna todos os tiers pertencentes a uma missão específica.
    /// </summary>
    /// <param name="missaoId">ID da missão.</param>
    /// <returns>Coleção de tiers da missão, ou lista vazia se não houver.</returns>
    Task<IEnumerable<Tier>> GetByMissaoIdAsync(Guid missaoId);
}
