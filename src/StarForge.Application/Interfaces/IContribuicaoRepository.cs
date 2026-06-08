using StarForge.Domain.Entities;

namespace StarForge.Application.Interfaces;

/// <summary>
/// Repositório especializado para a entidade <see cref="Contribuicao"/>.
/// </summary>
public interface IContribuicaoRepository : IRepositoryBase<Contribuicao>
{
    /// <summary>
    /// Retorna todas as contribuições realizadas por um usuário específico.
    /// Usado para exibir o histórico de contribuições do usuário logado.
    /// </summary>
    /// <param name="usuarioId">ID do usuário.</param>
    /// <returns>Contribuições do usuário em ordem cronológica.</returns>
    Task<IEnumerable<Contribuicao>> GetByUsuarioIdAsync(Guid usuarioId);

    /// <summary>
    /// Retorna todas as contribuições de uma missão específica.
    /// Usado para marcar contribuições pendentes como reembolso quando a missão falha.
    /// </summary>
    /// <param name="missaoId">ID da missão.</param>
    /// <returns>Contribuições da missão.</returns>
    Task<IEnumerable<Contribuicao>> GetByMissaoIdAsync(Guid missaoId);
}
