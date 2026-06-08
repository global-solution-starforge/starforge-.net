namespace StarForge.Application.Exceptions;

/// <summary>
/// Exceção lançada quando um recurso solicitado não é encontrado no banco de dados.
/// Mapeada pelo <c>ExceptionHandlingMiddleware</c> para HTTP 404 Not Found.
/// </summary>
/// <param name="entity">Nome da entidade que não foi encontrada (ex.: "Missao", "Usuario").</param>
/// <param name="key">Valor da chave buscada (geralmente um GUID).</param>
public class NotFoundException(string entity, object key)
    : Exception($"{entity} com identificador '{key}' não foi encontrado.");
