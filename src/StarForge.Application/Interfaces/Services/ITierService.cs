using StarForge.Application.DTOs.Tier;

namespace StarForge.Application.Interfaces.Services;

/// <summary>
/// Serviço responsável pelo gerenciamento dos tiers (níveis de recompensa) de uma missão.
/// </summary>
public interface ITierService
{
    /// <summary>Cria um novo tier em uma missão existente.</summary>
    /// <exception cref="Exceptions.NotFoundException">Lançada se a missão não existir.</exception>
    Task<TierDto> CriarAsync(CriarTierDto dto);

    /// <summary>Retorna os dados de um tier pelo ID.</summary>
    /// <exception cref="Exceptions.NotFoundException">Lançada se o tier não existir.</exception>
    Task<TierDto> GetByIdAsync(Guid id);

    /// <summary>Retorna todos os tiers de uma missão específica.</summary>
    Task<IEnumerable<TierDto>> GetByMissaoIdAsync(Guid missaoId);

    /// <summary>Remove permanentemente um tier.</summary>
    /// <exception cref="Exceptions.NotFoundException">Lançada se o tier não existir.</exception>
    Task DeletarAsync(Guid id);
}
