using System.ComponentModel.DataAnnotations;

namespace StarForge.Application.DTOs.Usuario;

/// <summary>
/// Dados necessários para registrar um novo piloto na plataforma StarForge.
/// Endpoint: <c>POST /api/usuarios</c> (acesso público — não requer autenticação).
/// </summary>
public record CriarUsuarioDto(
    /// <summary>Nome completo do piloto (mín. 3, máx. 100 caracteres).</summary>
    [Required][MinLength(3)][MaxLength(100)] string Nome,

    /// <summary>E-mail único de acesso. Utilizado como login.</summary>
    [Required][EmailAddress][MaxLength(100)] string Email,

    /// <summary>Senha em texto puro (mín. 6, máx. 100 caracteres). Será armazenada como hash BCrypt.</summary>
    [Required][MinLength(6)][MaxLength(100)] string Senha,

    /// <summary>
    /// Papel do usuário no sistema. Padrão: <c>"User"</c>.
    /// Use <c>"Admin"</c> para conceder permissões administrativas.
    /// Valores permitidos: <c>"User"</c> ou <c>"Admin"</c>.
    /// </summary>
    [MaxLength(20)][RegularExpression(@"^(User|Admin)$", ErrorMessage = "Role deve ser 'User' ou 'Admin'.")] string Role = "User"
);
