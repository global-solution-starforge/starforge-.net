using Microsoft.EntityFrameworkCore;
using StarForge.Application.Interfaces;
using StarForge.Infrastructure.Data;

namespace StarForge.Infrastructure.Repositories;

public abstract class RepositoryBase<T>(StarForgeDbContext context) : IRepositoryBase<T> where T : class
{
    protected readonly StarForgeDbContext _context = context;

    public async Task<T?> GetByIdAsync(Guid id) => await _context.Set<T>().FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

    public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);

    public void Update(T entity) => _context.Set<T>().Update(entity);

    public void Delete(T entity) => _context.Set<T>().Remove(entity);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
