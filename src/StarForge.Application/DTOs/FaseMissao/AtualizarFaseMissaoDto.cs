using System.ComponentModel.DataAnnotations;

namespace StarForge.Application.DTOs.FaseMissao;

/// <summary>
/// Dados editáveis de uma fase de missão.
/// Endpoint: <c>PUT /api/fases/{id}</c> (requer role Admin).
/// </summary>
public record AtualizarFaseMissaoDto(
    /// <summary>Novo título de exibição (máx. 100 caracteres).</summary>
    [Required][MaxLength(100)] string Titulo,

    /// <summary>Nova descrição da fase (máx. 500 caracteres).</summary>
    [Required][MaxLength(500)] string Descricao,

    /// <summary>Nova posição na sequência de fases.</summary>
    [Required][Range(1, int.MaxValue)] int Ordem
);
