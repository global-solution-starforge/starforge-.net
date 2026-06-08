using StarForge.Application.DTOs.Hangar;

namespace StarForge.Application.Interfaces.Services;

/// <summary>
/// Serviço de leitura do hangar do usuário — coleção de naves conquistadas.
/// Apenas consulta; o hangar é gerenciado internamente pelo <c>ContribuicaoService</c>.
/// </summary>
public interface IHangarService
{
    /// <summary>
    /// Retorna todas as naves (pendentes e desbloqueadas) do hangar de um usuário.
    /// </summary>
    /// <param name="usuarioId">ID do usuário dono do hangar.</param>
    /// <returns>Lista de hangares com detalhes da nave de cada um.</returns>
    Task<IEnumerable<HangarDto>> GetByUsuarioIdAsync(Guid usuarioId);

    /// <summary>Retorna um registro específico do hangar pelo ID.</summary>
    /// <exception cref="Exceptions.NotFoundException">Lançada se o hangar não existir.</exception>
    Task<HangarDto> GetByIdAsync(Guid id);
}
