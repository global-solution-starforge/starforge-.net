using System.ComponentModel.DataAnnotations;
using StarForge.Domain.Enums;

namespace StarForge.Application.DTOs.Contribuicao;

public record CriarContribuicaoDto(
    [Required] Guid UsuarioId,
    [Required] Guid MissaoId,
    [Required] Guid TierId,
    [Required][Range(0.01, double.MaxValue)] decimal Valor,
    [Required] MetodoPagamento MetodoPagamento
);
