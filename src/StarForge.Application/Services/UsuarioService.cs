using StarForge.Application.DTOs.Usuario;
using StarForge.Application.Exceptions;
using StarForge.Application.Interfaces;
using StarForge.Application.Interfaces.Services;
using StarForge.Domain.Entities;

namespace StarForge.Application.Services;

/// <summary>
/// Implementação do serviço de gerenciamento de usuários.
/// Responsável por cadastro, consulta, atualização de perfil e desativação de contas.
/// </summary>
/// <param name="usuarioRepo">Repositório de usuários.</param>
public class UsuarioService(IUsuarioRepository usuarioRepo) : IUsuarioService
{
    /// <inheritdoc />
    public async Task<UsuarioDto> CriarAsync(CriarUsuarioDto dto)
    {
        // Garante unicidade de e-mail antes de criar
        if (await usuarioRepo.GetByEmailAsync(dto.Email) is not null)
            throw new BusinessRuleException($"Já existe um usuário com o e-mail '{dto.Email}'.");

        // Gera o hash BCrypt antes de criar a entidade — nunca persiste senha em texto puro
        var hash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);
        var usuario = new Usuario(dto.Nome, dto.Email, hash, dto.Role);

        await usuarioRepo.AddAsync(usuario);
        await usuarioRepo.SaveChangesAsync();

        return MapToDto(usuario);
    }

    /// <inheritdoc />
    public async Task<UsuarioDto> GetByIdAsync(Guid id)
    {
        var usuario = await usuarioRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Usuario), id);
        return MapToDto(usuario);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UsuarioDto>> GetAllAsync()
    {
        var usuarios = await usuarioRepo.GetAllAsync();
        return usuarios.Select(MapToDto);
    }

    /// <inheritdoc />
    public async Task<UsuarioDto> AtualizarAsync(Guid id, AtualizarUsuarioDto dto)
    {
        var usuario = await usuarioRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Usuario), id);

        // Verifica unicidade do novo e-mail, excluindo o próprio usuário da comparação
        var emailExistente = await usuarioRepo.GetByEmailAsync(dto.Email);
        if (emailExistente is not null && emailExistente.Id != id)
            throw new BusinessRuleException($"Já existe um usuário com o e-mail '{dto.Email}'.");

        usuario.AtualizarDados(dto.Nome, dto.Email);
        usuarioRepo.Update(usuario);
        await usuarioRepo.SaveChangesAsync();

        return MapToDto(usuario);
    }

    /// <inheritdoc />
    public async Task DesativarAsync(Guid id)
    {
        var usuario = await usuarioRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Usuario), id);

        usuario.Desativar(); // Soft delete — preserva histórico de contribuições
        usuarioRepo.Update(usuario);
        await usuarioRepo.SaveChangesAsync();
    }

    /// <summary>
    /// Mapeia a entidade <see cref="Usuario"/> para o DTO de resposta, omitindo dados sensíveis.
    /// </summary>
    private static UsuarioDto MapToDto(Usuario u) =>
        new(u.Id, u.Nome, u.Email, u.Nivel, u.TotalContribuido, u.Ativo, u.Role, u.DataCadastro);
}
