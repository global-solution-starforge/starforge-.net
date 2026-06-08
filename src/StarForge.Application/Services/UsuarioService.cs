using StarForge.Application.DTOs.Usuario;
using StarForge.Application.Exceptions;
using StarForge.Application.Interfaces;
using StarForge.Application.Interfaces.Services;
using StarForge.Domain.Entities;

namespace StarForge.Application.Services;

public class UsuarioService(IUsuarioRepository usuarioRepo) : IUsuarioService
{
    public async Task<UsuarioDto> CriarAsync(CriarUsuarioDto dto)
    {
        if (await usuarioRepo.GetByEmailAsync(dto.Email) is not null)
            throw new BusinessRuleException($"Já existe um usuário com o e-mail '{dto.Email}'.");

        if (await usuarioRepo.GetByRmAsync(dto.Rm) is not null)
            throw new BusinessRuleException($"Já existe um usuário com o RM '{dto.Rm}'.");

        var hash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);
        var usuario = new Usuario(dto.Nome, dto.Email, dto.Rm, hash, dto.Role);

        await usuarioRepo.AddAsync(usuario);
        await usuarioRepo.SaveChangesAsync();

        return MapToDto(usuario);
    }

    public async Task<UsuarioDto> GetByIdAsync(Guid id)
    {
        var usuario = await usuarioRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Usuario), id);
        return MapToDto(usuario);
    }

    public async Task<IEnumerable<UsuarioDto>> GetAllAsync()
    {
        var usuarios = await usuarioRepo.GetAllAsync();
        return usuarios.Select(MapToDto);
    }

    public async Task<UsuarioDto> AtualizarAsync(Guid id, AtualizarUsuarioDto dto)
    {
        var usuario = await usuarioRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Usuario), id);

        var emailExistente = await usuarioRepo.GetByEmailAsync(dto.Email);
        if (emailExistente is not null && emailExistente.Id != id)
            throw new BusinessRuleException($"Já existe um usuário com o e-mail '{dto.Email}'.");

        usuario.AtualizarDados(dto.Nome, dto.Email);
        usuarioRepo.Update(usuario);
        await usuarioRepo.SaveChangesAsync();

        return MapToDto(usuario);
    }

    public async Task DesativarAsync(Guid id)
    {
        var usuario = await usuarioRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Usuario), id);

        usuario.Desativar();
        usuarioRepo.Update(usuario);
        await usuarioRepo.SaveChangesAsync();
    }

    private static UsuarioDto MapToDto(Usuario u) =>
        new(u.Id, u.Nome, u.Email, u.Rm, u.Nivel, u.TotalContribuido, u.Ativo, u.Role, u.DataCadastro);
}
