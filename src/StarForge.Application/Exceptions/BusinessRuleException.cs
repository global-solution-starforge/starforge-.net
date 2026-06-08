namespace StarForge.Application.Exceptions;

/// <summary>
/// Exceção lançada quando uma regra de negócio de orquestração é violada na camada de serviços.
/// Mapeada pelo <c>ExceptionHandlingMiddleware</c> para HTTP 422 Unprocessable Entity.
/// </summary>
/// <remarks>
/// Use esta exceção para validações de contexto entre entidades (ex.: missão não está ativa,
/// e-mail já cadastrado, datas inválidas). Para invariantes de uma única entidade, use
/// <c>DomainException</c> da camada Domain.
/// </remarks>
/// <param name="message">Descrição legível da regra violada, exibida na resposta da API.</param>
public class BusinessRuleException(string message) : Exception(message);
