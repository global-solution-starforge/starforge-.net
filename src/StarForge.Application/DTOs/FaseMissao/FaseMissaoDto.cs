using StarForge.Domain.Enums;

namespace StarForge.Application.DTOs.FaseMissao;

/// <summary>
/// DTO de resposta com os dados completos de uma fase de missão.
/// </summary>
public record FaseMissaoDto(
    /// <summary>Identificador único da fase.</summary>
    Guid Id,

    /// <summary>ID da missão proprietária.</summary>
    Guid MissaoId,

    /// <summary>Título de exibição da fase.</summary>
    string Titulo,

    /// <summary>Descrição do conteúdo narrativo da fase.</summary>
    string Descricao,

    /// <summary>Número de sequência de exibição dentro da missão.</summary>
    int Ordem,

    /// <summary>Status atual da fase (Pendente, EmAndamento ou Concluida).</summary>
    StatusFaseMissao Status,

    /// <summary>Data e hora (UTC) de conclusão. <c>null</c> se ainda não concluída.</summary>
    DateTime? DataConclusao
);
