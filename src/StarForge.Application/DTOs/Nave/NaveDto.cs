namespace StarForge.Application.DTOs.Nave;

public record NaveDto(
    Guid Id,
    string Nome,
    string Modelo,
    string Descricao,
    string Raridade,
    string? ImagemUrl,
    Guid MissaoId
);
