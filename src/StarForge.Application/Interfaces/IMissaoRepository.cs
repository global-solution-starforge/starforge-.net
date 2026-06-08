using StarForge.Domain.Entities;
using StarForge.Domain.Enums;

namespace StarForge.Application.Interfaces;

/// <summary>
/// Repositório especializado para a entidade <see cref="Missao"/>.
/// </summary>
public interface IMissaoRepository : IRepositoryBase<Missao>
{
    /// <summary>
    /// Retorna todas as missões que possuem um determinado status.
    /// </summary>
    /// <param name="status">Status desejado (Ativa, Concluida ou Falhou).</param>
    /// <returns>Coleção de missões no status informado.</returns>
    Task<IEnumerable<Missao>> GetByStatusAsync(StatusMissao status);
}
