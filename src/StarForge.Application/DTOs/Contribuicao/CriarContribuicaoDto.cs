using System.ComponentModel.DataAnnotations;
using StarForge.Domain.Enums;

namespace StarForge.Application.DTOs.Contribuicao;

/// <summary>
/// Dados necessários para criar uma nova contribuição financeira.
/// Endpoint: <c>POST /api/contribuicoes</c> (requer autenticação).
/// </summary>
public record CriarContribuicaoDto(
    /// <summary>ID do usuário que está contribuindo.</summary>
    [Required] Guid UsuarioId,

    /// <summary>ID da missão alvo da contribuição.</summary>
    [Required] Guid MissaoId,

    /// <summary>ID do tier (nível de recompensa) escolhido pelo contribuidor.</summary>
    [Required] Guid TierId,

    /// <summary>Valor em reais da contribuição. Deve ser maior que R$ 0,01.</summary>
    [Required][Range(0.01, double.MaxValue)] decimal Valor,

    /// <summary>Forma de pagamento selecionada. Deve ser um valor válido do enum <see cref="MetodoPagamento"/>.</summary>
    [Required][EnumDataType(typeof(MetodoPagamento))] MetodoPagamento MetodoPagamento
);
