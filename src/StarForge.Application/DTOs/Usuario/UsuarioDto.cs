namespace StarForge.Application.DTOs.Usuario;

public record UsuarioDto(
    Guid Id,
    string Nome,
    string Email,
    string Rm,
    string Nivel,
    decimal TotalContribuido,
    bool Ativo,
    string Role,
    DateTime DataCadastro
);
