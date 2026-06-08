using StarForge.Domain.Entities;

namespace StarForge.Application.Interfaces;

/// <summary>
/// Repositório especializado para a entidade <see cref="Hangar"/>.
/// </summary>
public interface IHangarRepository : IRepositoryBase<Hangar>
{
    /// <summary>
    /// Retorna todos os registros de hangar de um usuário (naves pendentes e desbloqueadas).
    /// Usado pelo endpoint de hangar do usuário logado e pelo cancelamento de contribuições.
    /// </summary>
    /// <param name="usuarioId">ID do usuário.</param>
    /// <returns>Hangares do usuário.</returns>
    Task<IEnumerable<Hangar>> GetByUsuarioIdAsync(Guid usuarioId);

    /// <summary>
    /// Retorna todos os hangares pendentes de uma missão.
    /// Usado para desbloquear todos os hangares quando a missão é concluída.
    /// </summary>
    /// <param name="missaoId">ID da missão.</param>
    /// <returns>Hangares associados à missão.</returns>
    Task<IEnumerable<Hangar>> GetByMissaoIdAsync(Guid missaoId);
}
