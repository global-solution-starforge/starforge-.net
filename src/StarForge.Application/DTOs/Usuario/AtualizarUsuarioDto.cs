using System.ComponentModel.DataAnnotations;

namespace StarForge.Application.DTOs.Usuario;

public record AtualizarUsuarioDto(
    [Required][MaxLength(100)] string Nome,
    [Required][EmailAddress][MaxLength(100)] string Email
);
