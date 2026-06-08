using System.ComponentModel.DataAnnotations;

namespace StarForge.Application.DTOs.FaseMissao;

/// <summary>
/// Dados necessários para criar uma nova fase em uma missão.
/// Endpoint: <c>POST /api/fases</c> (requer role Admin).
/// </summary>
public record CriarFaseMissaoDto(
    /// <summary>ID da missão à qual a fase pertence.</summary>
    [Required] Guid MissaoId,

    /// <summary>Título de exibição da fase (mín. 3, máx. 100 caracteres).</summary>
    [Required][MinLength(3)][MaxLength(100)] string Titulo,

    /// <summary>Descrição do que ocorre nesta fase (mín. 10, máx. 500 caracteres).</summary>
    [Required][MinLength(10)][MaxLength(500)] string Descricao,

    /// <summary>Número de sequência para ordenação (1, 2, 3...). Deve ser positivo.</summary>
    [Required][Range(1, int.MaxValue)] int Ordem
);
