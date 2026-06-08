using StarForge.Application.DTOs.Contribuicao;

namespace StarForge.Application.Interfaces.Services;

/// <summary>
/// Serviço responsável pelo ciclo de vida das contribuições financeiras.
/// Orquestra a interação entre Contribuicao, Tier, Usuario, Missao e Hangar.
/// </summary>
public interface IContribuicaoService
{
    /// <summary>
    /// Cria uma contribuição pendente e reserva a nave no hangar do usuário.
    /// </summary>
    /// <param name="dto">Dados da contribuição (missão, tier, valor, pagamento).</param>
    /// <returns>DTO da contribuição criada com status Pendente.</returns>
    /// <exception cref="Exceptions.NotFoundException">Lançada se missão, tier ou usuário não existirem.</exception>
    /// <exception cref="Exceptions.BusinessRuleException">
    /// Lançada se a missão não estiver ativa, se o tier não tiver vagas ou a missão não tiver naves.
    /// </exception>
    Task<ContribuicaoDto> CriarAsync(CriarContribuicaoDto dto);

    /// <summary>
    /// Confirma o pagamento da contribuição e executa a cadeia de efeitos colaterais:
    /// ocupa a vaga no tier, atualiza o nível do usuário, registra na missão,
    /// verifica se a meta foi atingida e desbloqueia hangares se necessário.
    /// </summary>
    /// <param name="id">ID da contribuição a confirmar.</param>
    /// <returns>DTO da contribuição com status Confirmada.</returns>
    /// <exception cref="Exceptions.NotFoundException">Lançada se alguma entidade relacionada não for encontrada.</exception>
    Task<ContribuicaoDto> ConfirmarAsync(Guid id);

    /// <summary>
    /// Cancela voluntariamente uma contribuição pendente e remove o hangar associado.
    /// </summary>
    /// <param name="id">ID da contribuição a cancelar.</param>
    /// <returns>DTO da contribuição com status Cancelada.</returns>
    /// <exception cref="Exceptions.NotFoundException">Lançada se a contribuição não existir.</exception>
    Task<ContribuicaoDto> CancelarAsync(Guid id);

    /// <summary>Retorna todas as contribuições realizadas por um usuário.</summary>
    Task<IEnumerable<ContribuicaoDto>> GetByUsuarioIdAsync(Guid usuarioId);
}
