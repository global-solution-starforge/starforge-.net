using StarForge.Application.DTOs.Usuario;

namespace StarForge.Application.DTOs.Auth;

public record TokenResponseDto(
    string Token,
    DateTime Expiration,
    UsuarioDto Usuario
);
