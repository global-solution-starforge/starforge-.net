using Microsoft.EntityFrameworkCore;
using StarForge.Application.Interfaces;
using StarForge.Domain.Entities;
using StarForge.Infrastructure.Data;

namespace StarForge.Infrastructure.Repositories;

/// <summary>
/// Repositório concreto para a entidade <see cref="Usuario"/>.
/// Herda as operações CRUD de <see cref="RepositoryBase{T}"/> e adiciona
/// consultas específicas de usuário.
/// </summary>
/// <param name="context">DbContext injetado pelo DI (Scoped por requisição).</param>
public class UsuarioRepository(StarForgeDbContext context) : RepositoryBase<Usuario>(context), IUsuarioRepository
{
    /// <inheritdoc />
    /// <remarks>
    /// Usado pelo <c>AuthService</c> para validar as credenciais no login
    /// e pelo <c>UsuarioService</c> para verificar unicidade antes de criar um usuário.
    /// </remarks>
    public async Task<Usuario?> GetByEmailAsync(string email) =>
        await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

}
