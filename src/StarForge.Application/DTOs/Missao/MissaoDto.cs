using StarForge.Domain.Enums;

namespace StarForge.Application.DTOs.Missao;

public record MissaoDto(
    Guid Id,
    string Nome,
    string Descricao,
    decimal Meta,
    decimal TotalArrecadado,
    decimal PercentualArrecadado,
    DateTime DataInicio,
    DateTime DataLimite,
    StatusMissao Status,
    string? ImagemUrl
);
