using StarForge.Domain.Enums;

namespace StarForge.Application.DTOs.FaseMissao;

public record FaseMissaoDto(
    Guid Id,
    Guid MissaoId,
    string Titulo,
    string Descricao,
    int Ordem,
    StatusFaseMissao Status,
    DateTime? DataConclusao
);
