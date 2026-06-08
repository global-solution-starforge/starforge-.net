using System.ComponentModel.DataAnnotations;

namespace StarForge.Application.DTOs.Missao;

/// <summary>
/// Dados necessários para criar ou atualizar uma missão espacial.
/// Endpoint: <c>POST /api/missoes</c> e <c>PUT /api/missoes/{id}</c> (requer role Admin).
/// </summary>
public record CriarMissaoDto(
    /// <summary>Nome de exibição da missão (mín. 3, máx. 100 caracteres).</summary>
    [Required][MinLength(3)][MaxLength(100)] string Nome,

    /// <summary>Descrição narrativa e técnica da missão (mín. 10, máx. 1000 caracteres).</summary>
    [Required][MinLength(10)][MaxLength(1000)] string Descricao,

    /// <summary>Meta financeira em reais. Deve ser maior que zero.</summary>
    [Required][Range(1, double.MaxValue)] decimal Meta,

    /// <summary>Data e hora (UTC) de início. Deve ser anterior a <see cref="DataLimite"/>.</summary>
    [Required] DateTime DataInicio,

    /// <summary>Data e hora (UTC) limite para atingir a meta. Após esta data, a missão falha.</summary>
    [Required] DateTime DataLimite,

    /// <summary>URL opcional da imagem de capa. Deve ser uma URL absoluta válida (máx. 255 caracteres).</summary>
    [Url][MaxLength(255)] string? ImagemUrl = null
);
