using System.ComponentModel.DataAnnotations;

namespace StarForge.Application.DTOs.Missao;

public record CriarMissaoDto(
    [Required][MaxLength(100)] string Nome,
    [Required][MaxLength(1000)] string Descricao,
    [Required][Range(1, double.MaxValue)] decimal Meta,
    [Required] DateTime DataInicio,
    [Required] DateTime DataLimite,
    [MaxLength(255)] string? ImagemUrl = null
);
