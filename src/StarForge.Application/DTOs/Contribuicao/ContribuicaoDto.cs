using StarForge.Domain.Enums;

namespace StarForge.Application.DTOs.Contribuicao;

/// <summary>
/// DTO de resposta com os dados completos de uma contribuição financeira.
/// </summary>
public record ContribuicaoDto(
    /// <summary>Identificador único da contribuição.</summary>
    Guid Id,

    /// <summary>ID do usuário que realizou a contribuição.</summary>
    Guid UsuarioId,

    /// <summary>ID da missão que recebeu a contribuição.</summary>
    Guid MissaoId,

    /// <summary>ID do tier escolhido pelo contribuidor.</summary>
    Guid TierId,

    /// <summary>Valor em reais da contribuição.</summary>
    decimal Valor,

    /// <summary>Status atual da contribuição (Pendente, Confirmada, Cancelada ou Reembolso).</summary>
    StatusContribuicao Status,

    /// <summary>Forma de pagamento utilizada.</summary>
    MetodoPagamento MetodoPagamento,

    /// <summary>Data e hora (UTC) em que a contribuição foi criada.</summary>
    DateTime DataContribuicao,

    /// <summary>Data e hora (UTC) da confirmação do pagamento. <c>null</c> se ainda pendente.</summary>
    DateTime? DataConfirmacao
);
