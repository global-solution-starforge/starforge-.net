using StarForge.Domain.Enums;

namespace StarForge.Application.DTOs.Contribuicao;

public record ContribuicaoDto(
    Guid Id,
    Guid UsuarioId,
    Guid MissaoId,
    Guid TierId,
    decimal Valor,
    StatusContribuicao Status,
    MetodoPagamento MetodoPagamento,
    DateTime DataContribuicao,
    DateTime? DataConfirmacao
);
