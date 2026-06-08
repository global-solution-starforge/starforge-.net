using StarForge.Domain.Enums;

namespace StarForge.Application.DTOs.Missao;

/// <summary>
/// DTO de resposta com os dados completos de uma missão, incluindo progresso calculado.
/// </summary>
public record MissaoDto(
    /// <summary>Identificador único da missão.</summary>
    Guid Id,

    /// <summary>Nome de exibição da missão.</summary>
    string Nome,

    /// <summary>Descrição narrativa da missão.</summary>
    string Descricao,

    /// <summary>Meta financeira em reais que a missão precisa atingir.</summary>
    decimal Meta,

    /// <summary>Total arrecadado até o momento com contribuições confirmadas.</summary>
    decimal TotalArrecadado,

    /// <summary>
    /// Percentual da meta já arrecadado (0 a 100+).
    /// Calculado como: <c>TotalArrecadado / Meta * 100</c>, arredondado em 2 casas decimais.
    /// </summary>
    decimal PercentualArrecadado,

    /// <summary>Data e hora (UTC) de início da missão.</summary>
    DateTime DataInicio,

    /// <summary>Data e hora (UTC) limite para arrecadar a meta.</summary>
    DateTime DataLimite,

    /// <summary>Status atual da missão no ciclo de vida.</summary>
    StatusMissao Status,

    /// <summary>URL da imagem de capa. Pode ser <c>null</c>.</summary>
    string? ImagemUrl
);
