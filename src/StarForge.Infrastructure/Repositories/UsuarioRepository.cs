using Microsoft.EntityFrameworkCore;
using StarForge.Application.Interfaces;
using StarForge.Domain.Entities;
using StarForge.Infrastructure.Data;

namespace StarForge.Infrastructure.Repositories;

public class UsuarioRepository(StarForgeDbContext context) : RepositoryBase<Usuario>(context), IUsuarioRepository
{
    public async Task<Usuario?> GetByEmailAsync(string email) =>
        await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<Usuario?> GetByRmAsync(string rm) =>
        await _context.Usuarios.FirstOrDefaultAsync(u => u.Rm == rm);
}
