using System.ComponentModel.DataAnnotations;

namespace StarForge.Application.DTOs.FaseMissao;

public record CriarFaseMissaoDto(
    [Required] Guid MissaoId,
    [Required][MaxLength(100)] string Titulo,
    [Required][MaxLength(500)] string Descricao,
    [Required][Range(1, int.MaxValue)] int Ordem
);
