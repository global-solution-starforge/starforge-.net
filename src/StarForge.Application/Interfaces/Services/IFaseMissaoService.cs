using StarForge.Application.DTOs.FaseMissao;

namespace StarForge.Application.Interfaces.Services;

/// <summary>
/// Serviço responsável pelo gerenciamento das fases/etapas narrativas de uma missão.
/// </summary>
public interface IFaseMissaoService
{
    /// <summary>Cria uma nova fase associada a uma missão existente.</summary>
    /// <exception cref="Exceptions.NotFoundException">Lançada se a missão não existir.</exception>
    Task<FaseMissaoDto> CriarAsync(CriarFaseMissaoDto dto);

    /// <summary>Retorna os dados de uma fase pelo ID.</summary>
    /// <exception cref="Exceptions.NotFoundException">Lançada se a fase não existir.</exception>
    Task<FaseMissaoDto> GetByIdAsync(Guid id);

    /// <summary>Retorna todas as fases de uma missão, ordenadas pelo campo Ordem.</summary>
    Task<IEnumerable<FaseMissaoDto>> GetByMissaoIdAsync(Guid missaoId);

    /// <summary>Atualiza título, descrição e ordem de uma fase existente.</summary>
    /// <exception cref="Exceptions.NotFoundException">Lançada se a fase não existir.</exception>
    Task<FaseMissaoDto> AtualizarAsync(Guid id, AtualizarFaseMissaoDto dto);

    /// <summary>
    /// Marca uma fase como concluída. Operação irreversível.
    /// </summary>
    /// <exception cref="Exceptions.NotFoundException">Lançada se a fase não existir.</exception>
    /// <exception cref="StarForge.Domain.Exceptions.DomainException">Lançada se a fase já estiver concluída.</exception>
    Task<FaseMissaoDto> ConcluirAsync(Guid id);

    /// <summary>Remove permanentemente uma fase.</summary>
    /// <exception cref="Exceptions.NotFoundException">Lançada se a fase não existir.</exception>
    Task DeletarAsync(Guid id);
}
