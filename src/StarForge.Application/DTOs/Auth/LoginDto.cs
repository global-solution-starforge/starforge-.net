using System.ComponentModel.DataAnnotations;

namespace StarForge.Application.DTOs.Auth;

/// <summary>
/// Dados de entrada para o endpoint de autenticação <c>POST /api/auth/login</c>.
/// </summary>
public record LoginDto(
    /// <summary>E-mail cadastrado na plataforma. Deve ser um endereço válido.</summary>
    [Required][EmailAddress] string Email,

    /// <summary>Senha em texto puro — comparada via BCrypt contra o hash armazenado (mín. 6, máx. 100 caracteres).</summary>
    [Required][MinLength(6)][MaxLength(100)] string Senha
);
