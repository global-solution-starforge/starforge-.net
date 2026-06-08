namespace StarForge.Application.Interfaces;

/// <summary>
/// Contrato genérico de repositório para operações CRUD comuns a todas as entidades.
/// As implementações ficam na camada Infrastructure — a Application só depende desta interface.
/// </summary>
/// <typeparam name="T">Tipo da entidade de domínio gerenciada pelo repositório.</typeparam>
public interface IRepositoryBase<T> where T : class
{
    /// <summary>
    /// Busca uma entidade pelo seu identificador único.
    /// </summary>
    /// <param name="id">GUID da entidade desejada.</param>
    /// <returns>A entidade encontrada, ou <c>null</c> se não existir.</returns>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retorna todas as entidades do tipo <typeparamref name="T"/> no banco de dados.
    /// </summary>
    /// <returns>Coleção (possivelmente vazia) de entidades.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Adiciona uma nova entidade ao contexto para persistência.
    /// A entidade só é salva no banco após <see cref="SaveChangesAsync"/> ser chamado.
    /// </summary>
    /// <param name="entity">Entidade a ser inserida.</param>
    Task AddAsync(T entity);

    /// <summary>
    /// Marca a entidade como modificada no contexto do EF Core.
    /// As alterações são persistidas quando <see cref="SaveChangesAsync"/> for chamado.
    /// </summary>
    /// <param name="entity">Entidade com valores atualizados.</param>
    void Update(T entity);

    /// <summary>
    /// Marca a entidade para exclusão no contexto do EF Core.
    /// A remoção ocorre no banco quando <see cref="SaveChangesAsync"/> for chamado.
    /// </summary>
    /// <param name="entity">Entidade a ser removida.</param>
    void Delete(T entity);

    /// <summary>
    /// Persiste todas as alterações pendentes (Add, Update, Delete) no banco de dados.
    /// Deve ser chamado ao final de cada operação de escrita no serviço.
    /// </summary>
    Task SaveChangesAsync();
}
