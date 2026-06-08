using StarForge.Application.DTOs.Nave;
using StarForge.Domain.Enums;

namespace StarForge.Application.DTOs.Hangar;

/// <summary>
/// DTO de resposta que representa uma entrada no hangar do usuário,
/// incluindo os detalhes completos da nave conquistada.
/// </summary>
public record HangarDto(
    /// <summary>Identificador único do registro de hangar.</summary>
    Guid Id,

    /// <summary>ID do usuário dono desta nave.</summary>
    Guid UsuarioId,

    /// <summary>ID da missão que originou esta nave.</summary>
    Guid MissaoId,

    /// <summary>Status da nave: <c>Pendente</c> (missão em andamento) ou <c>Desbloqueada</c> (missão concluída).</summary>
    StatusHangar Status,

    /// <summary>Data e hora (UTC) em que o registro foi criado (ao confirmar a contribuição).</summary>
    DateTime DataAquisicao,

    /// <summary>Dados completos da nave contida neste hangar.</summary>
    NaveDto Nave
);
