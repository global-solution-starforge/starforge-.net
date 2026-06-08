namespace StarForge.Application.Exceptions;

public class NotFoundException(string entity, object key)
    : Exception($"{entity} com identificador '{key}' não foi encontrado.");
