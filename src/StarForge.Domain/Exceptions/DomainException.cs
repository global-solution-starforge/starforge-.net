namespace StarForge.Domain.Exceptions;

/// <summary>
/// Exceção lançada por regras de negócio do domínio.
/// </summary>
public class DomainException(string message) : Exception(message);
