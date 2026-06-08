using StarForge.Application.DTOs.Missao;

namespace StarForge.Application.Interfaces.Services;

/// <summary>
/// Serviço responsável pelo gerenciamento do ciclo de vida das missões de crowdfunding.
/// </summary>
public interface IMissaoService
{
    /// <summary>
    /// Cria uma nova missão com status inicial Ativa.
    /// </summary>
    /// <param name="dto">Dados da missão.</param>
    /// <returns>DTO da missão criada.</returns>
    /// <exception cref="Exceptions.BusinessRuleException">
    /// Lançada se <c>DataInicio</c> for maior ou igual a <c>DataLimite</c>.
    /// </exception>
    Task<MissaoDto> CriarAsync(CriarMissaoDto dto);

    /// <summary>Retorna uma missão pelo ID.</summary>
    /// <exception cref="Exceptions.NotFoundException">Lançada se a missão não existir.</exception>
    Task<MissaoDto> GetByIdAsync(Guid id);

    /// <summary>Retorna todas as missões cadastradas.</summary>
    Task<IEnumerable<MissaoDto>> GetAllAsync();

    /// <summary>Retorna apenas as missões com status <c>Ativa</c>.</summary>
    Task<IEnumerable<MissaoDto>> GetAtivasAsync();

    /// <summary>Atualiza os dados de uma missão existente.</summary>
    /// <exception cref="Exceptions.NotFoundException">Lançada se a missão não existir.</exception>
    Task<MissaoDto> AtualizarAsync(Guid id, CriarMissaoDto dto);

    /// <summary>Remove permanentemente uma missão do banco de dados.</summary>
    /// <exception cref="Exceptions.NotFoundException">Lançada se a missão não existir.</exception>
    Task DeletarAsync(Guid id);
}
