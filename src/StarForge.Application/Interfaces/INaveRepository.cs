using StarForge.Domain.Entities;

namespace StarForge.Application.Interfaces;

/// <summary>
/// Repositório especializado para a entidade <see cref="Nave"/>.
/// </summary>
public interface INaveRepository : IRepositoryBase<Nave>
{
    /// <summary>
    /// Retorna todas as naves associadas a uma missão específica.
    /// </summary>
    /// <param name="missaoId">ID da missão.</param>
    /// <returns>Coleção de naves da missão, ou lista vazia se não houver.</returns>
    Task<IEnumerable<Nave>> GetByMissaoIdAsync(Guid missaoId);
}
