using StarForge.Domain.Entities;

namespace StarForge.Application.Interfaces;

public interface IUsuarioRepository : IRepositoryBase<Usuario>
{
    Task<Usuario?> GetByEmailAsync(string email);
    Task<Usuario?> GetByRmAsync(string rm);
}
