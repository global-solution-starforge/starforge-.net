using StarForge.Domain.Entities;

namespace StarForge.Application.Interfaces;

/// <summary>
/// Repositório especializado para a entidade <see cref="Usuario"/>.
/// Estende <see cref="IRepositoryBase{T}"/> com consultas específicas de negócio.
/// </summary>
public interface IUsuarioRepository : IRepositoryBase<Usuario>
{
    /// <summary>
    /// Busca um usuário pelo e-mail de cadastro.
    /// Utilizado no login e para verificar unicidade ao criar/atualizar conta.
    /// </summary>
    /// <param name="email">E-mail do usuário (case-insensitive na implementação Oracle).</param>
    /// <returns>O usuário encontrado, ou <c>null</c> se não existir.</returns>
    Task<Usuario?> GetByEmailAsync(string email);


}
