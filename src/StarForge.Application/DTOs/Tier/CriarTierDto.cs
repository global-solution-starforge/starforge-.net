using System.ComponentModel.DataAnnotations;

namespace StarForge.Application.DTOs.Tier;

public record CriarTierDto(
    [Required][MaxLength(50)] string Nome,
    [Required][Range(1, double.MaxValue)] decimal Valor,
    [Required][MaxLength(255)] string BeneficioDescricao,
    [Required][Range(1, int.MaxValue)] int LimiteVagas,
    [Required] Guid MissaoId
);
