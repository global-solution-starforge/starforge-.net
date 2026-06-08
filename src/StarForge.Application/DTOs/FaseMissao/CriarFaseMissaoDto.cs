using System.ComponentModel.DataAnnotations;

namespace StarForge.Application.DTOs.FaseMissao;

/// <summary>
/// Dados necessários para criar uma nova fase em uma missão.
/// Endpoint: <c>POST /api/fases</c> (requer role Admin).
/// </summary>
public record CriarFaseMissaoDto(
    /// <summary>ID da missão à qual a fase pertence.</summary>
    [Required] Guid MissaoId,

    /// <summary>Título de exibição da fase (máx. 100 caracteres).</summary>
    [Required][MaxLength(100)] string Titulo,

    /// <summary>Descrição do que ocorre nesta fase (máx. 500 caracteres).</summary>
    [Required][MaxLength(500)] string Descricao,

    /// <summary>Número de sequência para ordenação (1, 2, 3...). Deve ser positivo.</summary>
    [Required][Range(1, int.MaxValue)] int Ordem
);
