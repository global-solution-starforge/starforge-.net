namespace StarForge.Application.Exceptions;

/// <summary>
/// Exceção lançada quando há falhas de validação em campos do request (DataAnnotations ou validação manual).
/// Mapeada pelo <c>ExceptionHandlingMiddleware</c> para HTTP 400 Bad Request com detalhes por campo.
/// </summary>
/// <param name="errors">
/// Dicionário de erros onde a chave é o nome do campo e o valor é um array de mensagens de erro.
/// Exemplo: <c>{ "Email": ["Campo obrigatório", "Formato inválido"] }</c>
/// </param>
public class ValidationException(IDictionary<string, string[]> errors)
    : Exception("Um ou mais erros de validação ocorreram.")
{
    /// <summary>
    /// Erros de validação por campo, incluídos na resposta RFC 7807 sob a chave <c>"errors"</c>.
    /// </summary>
    public IDictionary<string, string[]> Errors { get; } = errors;
}
