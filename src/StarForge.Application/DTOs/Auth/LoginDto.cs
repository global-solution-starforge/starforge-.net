using System.ComponentModel.DataAnnotations;

namespace StarForge.Application.DTOs.Auth;

public record LoginDto(
    [Required][EmailAddress] string Email,
    [Required] string Senha
);
