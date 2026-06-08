using Microsoft.EntityFrameworkCore;
using StarForge.Application.Interfaces;
using StarForge.Infrastructure.Data;

namespace StarForge.Infrastructure.Repositories;

/// <summary>
/// Implementação genérica de repositório usando EF Core.
/// Fornece as operações CRUD básicas para qualquer entidade de domínio.
/// </summary>
/// <remarks>
/// Cada repositório concreto herda desta classe e adiciona apenas os métodos de consulta específicos.
/// O padrão de <c>SaveChangesAsync</c> separado permite orquestrar múltiplas operações em uma
/// única transação implícita do EF Core dentro de um serviço.
/// </remarks>
/// <typeparam name="T">Tipo da entidade de domínio gerenciada.</typeparam>
/// <param name="context">DbContext injetado pelo DI — Scoped por requisição.</param>
public abstract class RepositoryBase<T>(StarForgeDbContext context) : IRepositoryBase<T> where T : class
{
    /// <summary>Instância do DbContext compartilhada entre os métodos do repositório.</summary>
    protected readonly StarForgeDbContext _context = context;

    /// <inheritdoc />
    /// <remarks>Usa <c>FindAsync</c> que busca primeiro no cache de identidade do EF antes de ir ao banco.</remarks>
    public async Task<T?> GetByIdAsync(Guid id) => await _context.Set<T>().FindAsync(id);

    /// <inheritdoc />
    public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

    /// <inheritdoc />
    public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);

    /// <inheritdoc />
    /// <remarks>Marca o estado da entidade como <c>Modified</c> no Change Tracker do EF Core.</remarks>
    public void Update(T entity) => _context.Set<T>().Update(entity);

    /// <inheritdoc />
    /// <remarks>Marca o estado da entidade como <c>Deleted</c> no Change Tracker do EF Core.</remarks>
    public void Delete(T entity) => _context.Set<T>().Remove(entity);

    /// <inheritdoc />
    /// <remarks>Persiste todas as alterações pendentes em uma única chamada ao banco.</remarks>
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
