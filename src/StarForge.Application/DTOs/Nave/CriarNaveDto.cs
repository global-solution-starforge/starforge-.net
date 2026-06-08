using System.ComponentModel.DataAnnotations;

namespace StarForge.Application.DTOs.Nave;

/// <summary>
/// Dados necessários para cadastrar uma nave espacial associada a uma missão.
/// Endpoint: <c>POST /api/missoes/{missaoId}/naves</c> (requer role Admin).
/// </summary>
public record CriarNaveDto(
    /// <summary>Nome da nave (ex.: "Aurora Prime"). Mín. 2, máx. 100 caracteres.</summary>
    [Required][MinLength(2)][MaxLength(100)] string Nome,

    /// <summary>Modelo/classe técnica da nave (ex.: "Classe A"). Mín. 2, máx. 50 caracteres.</summary>
    [Required][MinLength(2)][MaxLength(50)] string Modelo,

    /// <summary>Descrição narrativa da nave (mín. 10, máx. 500 caracteres).</summary>
    [Required][MinLength(10)][MaxLength(500)] string Descricao,

    /// <summary>Raridade da nave como texto (ex.: "Comum", "Raro", "Épico"). Máx. 20 caracteres.</summary>
    [Required][MaxLength(20)] string Raridade,

    /// <summary>ID da missão à qual a nave pertence como recompensa.</summary>
    [Required] Guid MissaoId,

    /// <summary>URL opcional da imagem de arte da nave. Deve ser uma URL absoluta válida (máx. 255 caracteres).</summary>
    [Url][MaxLength(255)] string? ImagemUrl = null
);
