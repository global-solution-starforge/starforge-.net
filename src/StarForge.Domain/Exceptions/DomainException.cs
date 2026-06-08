namespace StarForge.Domain.Exceptions;

/// <summary>
/// Exceção lançada quando uma regra de invariante do domínio é violada diretamente em uma entidade.
/// </summary>
/// <remarks>
/// Use <see cref="DomainException"/> para violações de estado interno de entidades (ex.: tentar confirmar
/// uma contribuição já confirmada, concluir uma fase já concluída, ocupar vaga em tier esgotado).
/// <para>
/// Para regras de negócio de orquestração nos serviços de aplicação (ex.: missão não está ativa,
/// e-mail já cadastrado, datas inválidas), use <c>BusinessRuleException</c> da camada Application.
/// </para>
/// </remarks>
/// <param name="message">Mensagem descritiva da invariante violada.</param>
public class DomainException(string message) : Exception(message);
