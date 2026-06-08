namespace StarForge.Application.DTOs.Nave;

/// <summary>
/// DTO de resposta com os dados completos de uma nave espacial.
/// </summary>
public record NaveDto(
    /// <summary>Identificador único da nave.</summary>
    Guid Id,

    /// <summary>Nome de exibição da nave.</summary>
    string Nome,

    /// <summary>Modelo ou classe técnica.</summary>
    string Modelo,

    /// <summary>Descrição narrativa e técnica.</summary>
    string Descricao,

    /// <summary>Raridade (ex.: "Comum", "Raro", "Épico", "Lendário").</summary>
    string Raridade,

    /// <summary>URL da imagem de arte. Pode ser <c>null</c>.</summary>
    string? ImagemUrl,

    /// <summary>ID da missão à qual esta nave pertence.</summary>
    Guid MissaoId
);
