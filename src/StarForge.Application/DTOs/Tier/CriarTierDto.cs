using System.ComponentModel.DataAnnotations;

namespace StarForge.Application.DTOs.Tier;

/// <summary>
/// Dados necessários para criar um tier (nível de recompensa) em uma missão.
/// Endpoint: <c>POST /api/missoes/{missaoId}/tiers</c> (requer role Admin).
/// </summary>
public record CriarTierDto(
    /// <summary>Nome do tier (ex.: "Pioneiro", "Elite"). Máx. 50 caracteres.</summary>
    [Required][MaxLength(50)] string Nome,

    /// <summary>Valor em reais para participar deste tier. Mínimo R$ 1,00.</summary>
    [Required][Range(1, double.MaxValue)] decimal Valor,

    /// <summary>Descrição dos benefícios e recompensas concedidos por este tier (máx. 255 caracteres).</summary>
    [Required][MaxLength(255)] string BeneficioDescricao,

    /// <summary>Número máximo de vagas disponíveis. Mínimo 1.</summary>
    [Required][Range(1, int.MaxValue)] int LimiteVagas,

    /// <summary>ID da missão à qual o tier pertence.</summary>
    [Required] Guid MissaoId
);
