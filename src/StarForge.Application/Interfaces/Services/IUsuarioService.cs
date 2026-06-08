using StarForge.Application.DTOs.Usuario;

namespace StarForge.Application.Interfaces.Services;

public interface IUsuarioService
{
    Task<UsuarioDto> CriarAsync(CriarUsuarioDto dto);
    Task<UsuarioDto> GetByIdAsync(Guid id);
    Task<IEnumerable<UsuarioDto>> GetAllAsync();
    Task<UsuarioDto> AtualizarAsync(Guid id, AtualizarUsuarioDto dto);
    Task DesativarAsync(Guid id);
}
