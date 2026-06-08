using StarForge.Application.DTOs.Nave;
using StarForge.Domain.Enums;

namespace StarForge.Application.DTOs.Hangar;

public record HangarDto(
    Guid Id,
    Guid UsuarioId,
    Guid MissaoId,
    StatusHangar Status,
    DateTime DataAquisicao,
    NaveDto Nave
);
