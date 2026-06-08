using System.ComponentModel.DataAnnotations;

namespace StarForge.Application.DTOs.FaseMissao;

/// <summary>
/// Dados editáveis de uma fase de missão.
/// Endpoint: <c>PUT /api/fases/{id}</c> (requer role Admin).
/// </summary>
public record AtualizarFaseMissaoDto(
    /// <summary>Novo título de exibição (mín. 3, máx. 100 caracteres).</summary>
    [Required][MinLength(3)][MaxLength(100)] string Titulo,

    /// <summary>Nova descrição da fase (mín. 10, máx. 500 caracteres).</summary>
    [Required][MinLength(10)][MaxLength(500)] string Descricao,

    /// <summary>Nova posição na sequência de fases.</summary>
    [Required][Range(1, int.MaxValue)] int Ordem
);
