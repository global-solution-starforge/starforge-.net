using System.ComponentModel.DataAnnotations;

namespace StarForge.Application.DTOs.Usuario;

/// <summary>
/// Dados permitidos para atualização de perfil pelo próprio usuário.
/// Apenas nome e e-mail podem ser alterados via esta operação.
/// </summary>
public record AtualizarUsuarioDto(
    /// <summary>Novo nome completo (máx. 100 caracteres).</summary>
    [Required][MaxLength(100)] string Nome,

    /// <summary>Novo e-mail. Deve ser único na plataforma e em formato válido.</summary>
    [Required][EmailAddress][MaxLength(100)] string Email
);
