using StarForge.Application.DTOs.Usuario;

namespace StarForge.Application.DTOs.Auth;

/// <summary>
/// Resposta retornada após autenticação bem-sucedida.
/// O cliente deve incluir o token no header <c>Authorization: Bearer {Token}</c> em cada requisição autenticada.
/// </summary>
public record TokenResponseDto(
    /// <summary>Token JWT assinado com HMAC-SHA256. Válido pelo tempo configurado em <c>Jwt:ExpirationHours</c>.</summary>
    string Token,

    /// <summary>Data e hora (UTC) em que o token expira.</summary>
    DateTime Expiration,

    /// <summary>Dados públicos do usuário autenticado (ID, nível, role etc.).</summary>
    UsuarioDto Usuario
);
