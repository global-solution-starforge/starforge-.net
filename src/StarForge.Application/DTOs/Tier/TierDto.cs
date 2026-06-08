namespace StarForge.Application.DTOs.Tier;

/// <summary>
/// DTO de resposta com os dados completos de um tier, incluindo vagas disponíveis em tempo real.
/// </summary>
public record TierDto(
    /// <summary>Identificador único do tier.</summary>
    Guid Id,

    /// <summary>Nome de exibição do tier.</summary>
    string Nome,

    /// <summary>Valor em reais necessário para participar deste tier.</summary>
    decimal Valor,

    /// <summary>Descrição dos benefícios concedidos.</summary>
    string BeneficioDescricao,

    /// <summary>Capacidade máxima de vagas.</summary>
    int LimiteVagas,

    /// <summary>Vagas já preenchidas por contribuições confirmadas. <c>LimiteVagas - VagasOcupadas</c> = vagas restantes.</summary>
    int VagasOcupadas,

    /// <summary>ID da missão proprietária do tier.</summary>
    Guid MissaoId
);
