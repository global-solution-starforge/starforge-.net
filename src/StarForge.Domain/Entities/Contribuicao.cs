using StarForge.Domain.Enums;
using StarForge.Domain.Exceptions;

namespace StarForge.Domain.Entities;

/// <summary>
/// Representa uma contribuição financeira realizada por um usuário para uma missão.
/// </summary>
/// <remarks>
/// Máquina de estados:
/// <list type="bullet">
///   <item><term>Pendente → Confirmada</term><description>Pagamento processado com sucesso.</description></item>
///   <item><term>Pendente → Cancelada</term><description>Contribuição cancelada voluntariamente pelo usuário.</description></item>
///   <item><term>Pendente → Reembolso</term><description>Missão falhou antes da confirmação — valor deve ser devolvido.</description></item>
/// </list>
/// </remarks>
public class Contribuicao
{
    /// <summary>Identificador único da contribuição (chave primária).</summary>
    public Guid Id { get; private set; }

    /// <summary>FK do usuário que realizou a contribuição.</summary>
    public Guid UsuarioId { get; private set; }

    /// <summary>FK da missão para a qual a contribuição foi feita.</summary>
    public Guid MissaoId { get; private set; }

    /// <summary>FK do tier escolhido pelo contribuidor.</summary>
    public Guid TierId { get; private set; }

    /// <summary>Valor em reais da contribuição.</summary>
    public decimal Valor { get; private set; }

    /// <summary>Status atual da contribuição no ciclo de vida.</summary>
    public StatusContribuicao Status { get; private set; }

    /// <summary>Meio de pagamento utilizado para esta contribuição.</summary>
    public MetodoPagamento MetodoPagamento { get; private set; }

    /// <summary>Data e hora (UTC) em que a contribuição foi criada.</summary>
    public DateTime DataContribuicao { get; private set; }

    /// <summary>Data e hora (UTC) da confirmação do pagamento. <c>null</c> se ainda pendente.</summary>
    public DateTime? DataConfirmacao { get; private set; }

    /// <summary>Referência de navegação para o usuário contribuidor.</summary>
    public Usuario Usuario { get; private set; } = null!;

    /// <summary>Referência de navegação para a missão alvo.</summary>
    public Missao Missao { get; private set; } = null!;

    /// <summary>Referência de navegação para o tier selecionado.</summary>
    public Tier Tier { get; private set; } = null!;

    /// <summary>
    /// Construtor privado sem parâmetros exigido pelo EF Core.
    /// </summary>
    private Contribuicao()
    {
    }

    /// <summary>
    /// Cria uma nova contribuição com status inicial <see cref="StatusContribuicao.Pendente"/>.
    /// </summary>
    /// <param name="usuarioId">ID do usuário contribuidor.</param>
    /// <param name="missaoId">ID da missão que receberá a contribuição.</param>
    /// <param name="tierId">ID do tier escolhido pelo usuário.</param>
    /// <param name="valor">Valor em reais da contribuição.</param>
    /// <param name="metodoPagamento">Forma de pagamento selecionada.</param>
    public Contribuicao(Guid usuarioId, Guid missaoId, Guid tierId, decimal valor, MetodoPagamento metodoPagamento)
    {
        Id = Guid.NewGuid();
        UsuarioId = usuarioId;
        MissaoId = missaoId;
        TierId = tierId;
        Valor = valor;
        MetodoPagamento = metodoPagamento;
        Status = StatusContribuicao.Pendente; // Aguarda processamento do pagamento
        DataContribuicao = DateTime.UtcNow;
    }

    /// <summary>
    /// Confirma o pagamento da contribuição e registra a data/hora da confirmação.
    /// Dispara a cadeia de efeitos no serviço: ocupar vaga no tier, atualizar nível do usuário
    /// e registrar na missão.
    /// </summary>
    /// <exception cref="DomainException">
    /// Lançada se a contribuição não estiver no status <see cref="StatusContribuicao.Pendente"/>.
    /// </exception>
    public void Confirmar()
    {
        if (Status != StatusContribuicao.Pendente)
            throw new DomainException("Apenas contribuições pendentes podem ser confirmadas.");

        Status = StatusContribuicao.Confirmada;
        DataConfirmacao = DateTime.UtcNow; // Registra o momento exato da confirmação
    }

    /// <summary>
    /// Cancela a contribuição voluntariamente pelo usuário.
    /// O hangar pendente associado é removido pelo serviço após esta chamada.
    /// </summary>
    /// <exception cref="DomainException">
    /// Lançada se a contribuição já estiver cancelada.
    /// </exception>
    public void Cancelar()
    {
        if (Status == StatusContribuicao.Cancelada)
            throw new DomainException("A contribuição já está cancelada.");

        Status = StatusContribuicao.Cancelada;
    }

    /// <summary>
    /// Marca a contribuição para reembolso quando a missão falha antes de atingir a meta.
    /// Diferente de <see cref="Cancelar"/> — indica que o valor deve ser devolvido ao usuário,
    /// não que o usuário desistiu voluntariamente.
    /// </summary>
    /// <exception cref="DomainException">
    /// Lançada se a contribuição não estiver com status <see cref="StatusContribuicao.Pendente"/>.
    /// Contribuições já confirmadas não podem receber reembolso por esta via.
    /// </exception>
    public void MarcarReembolso()
    {
        if (Status != StatusContribuicao.Pendente)
            throw new DomainException("Só contribuições pendentes podem ser marcadas para reembolso.");

        Status = StatusContribuicao.Reembolso;
    }
}
