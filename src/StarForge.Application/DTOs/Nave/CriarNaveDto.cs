using System.ComponentModel.DataAnnotations;

namespace StarForge.Application.DTOs.Nave;

public record CriarNaveDto(
    [Required][MaxLength(100)] string Nome,
    [Required][MaxLength(50)] string Modelo,
    [Required][MaxLength(500)] string Descricao,
    [Required][MaxLength(20)] string Raridade,
    [Required] Guid MissaoId,
    [MaxLength(255)] string? ImagemUrl = null
);
