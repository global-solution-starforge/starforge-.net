using System.ComponentModel.DataAnnotations;

namespace StarForge.Application.DTOs.Usuario;

public record CriarUsuarioDto(
    [Required][MaxLength(100)] string Nome,
    [Required][EmailAddress][MaxLength(100)] string Email,
    [Required][MaxLength(20)] string Rm,
    [Required][MinLength(6)][MaxLength(100)] string Senha,
    string Role = "User"
);
