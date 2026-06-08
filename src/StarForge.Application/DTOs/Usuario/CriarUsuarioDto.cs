using System.ComponentModel.DataAnnotations;

namespace StarForge.Application.DTOs.Usuario;

/// <summary>
/// Dados necessários para registrar um novo piloto na plataforma StarForge.
/// Endpoint: <c>POST /api/usuarios</c> (acesso público — não requer autenticação).
/// </summary>
public record CriarUsuarioDto(
    /// <summary>Nome completo do piloto (máx. 100 caracteres).</summary>
    [Required][MaxLength(100)] string Nome,

    /// <summary>E-mail único de acesso. Utilizado como login.</summary>
    [Required][EmailAddress][MaxLength(100)] string Email,

    /// <summary>RM do aluno FIAP (máx. 20 caracteres). Deve ser único na plataforma.</summary>
    [Required][MaxLength(20)] string Rm,

    /// <summary>Senha em texto puro (mín. 6, máx. 100 caracteres). Será armazenada como hash BCrypt.</summary>
    [Required][MinLength(6)][MaxLength(100)] string Senha,

    /// <summary>
    /// Papel do usuário no sistema. Padrão: <c>"User"</c>.
    /// Use <c>"Admin"</c> para conceder permissões administrativas.
    /// </summary>
    string Role = "User"
);
