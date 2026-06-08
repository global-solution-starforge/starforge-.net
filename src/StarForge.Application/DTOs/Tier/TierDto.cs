namespace StarForge.Application.DTOs.Tier;

public record TierDto(
    Guid Id,
    string Nome,
    decimal Valor,
    string BeneficioDescricao,
    int LimiteVagas,
    int VagasOcupadas,
    Guid MissaoId
);
