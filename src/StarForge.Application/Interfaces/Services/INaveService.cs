using StarForge.Application.DTOs.Nave;

namespace StarForge.Application.Interfaces.Services;

/// <summary>
/// Serviço responsável pelo gerenciamento das naves associadas às missões.
/// </summary>
public interface INaveService
{
    /// <summary>Cria uma nova nave associada a uma missão existente.</summary>
    /// <exception cref="Exceptions.NotFoundException">Lançada se a missão não existir.</exception>
    Task<NaveDto> CriarAsync(CriarNaveDto dto);

    /// <summary>Retorna os dados de uma nave pelo ID.</summary>
    /// <exception cref="Exceptions.NotFoundException">Lançada se a nave não existir.</exception>
    Task<NaveDto> GetByIdAsync(Guid id);

    /// <summary>Retorna todas as naves de uma missão específica.</summary>
    Task<IEnumerable<NaveDto>> GetByMissaoIdAsync(Guid missaoId);

    /// <summary>Remove permanentemente uma nave.</summary>
    /// <exception cref="Exceptions.NotFoundException">Lançada se a nave não existir.</exception>
    Task DeletarAsync(Guid id);
}
