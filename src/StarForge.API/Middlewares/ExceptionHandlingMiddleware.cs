using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using StarForge.Application.Exceptions;
using AppValidationException = StarForge.Application.Exceptions.ValidationException;

namespace StarForge.API.Middlewares;

/// <summary>
/// Middleware de tratamento centralizado de exceções.
/// Intercepta qualquer exceção não tratada no pipeline e retorna uma resposta
/// <c>ProblemDetails</c> padronizada (RFC 7807).
/// </summary>
/// <remarks>
/// Mapeamento de exceções para status HTTP:
/// <list type="table">
///   <listheader><term>Exceção</term><description>HTTP Status</description></listheader>
///   <item><term><see cref="NotFoundException"/></term><description>404 Not Found</description></item>
///   <item><term><see cref="BusinessRuleException"/></term><description>422 Unprocessable Entity</description></item>
///   <item><term><see cref="AppValidationException"/></term><description>400 Bad Request com campos de erros</description></item>
///   <item><term>Demais <see cref="Exception"/></term><description>500 Internal Server Error</description></item>
/// </list>
/// Registrado ANTES de <c>RequestLoggingMiddleware</c> no pipeline para garantir
/// que erros durante o logging não quebrem o tratamento.
/// </remarks>
/// <param name="next">Próximo middleware no pipeline ASP.NET Core.</param>
/// <param name="logger">Logger injetado para registrar a stack trace completa.</param>
public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    /// <summary>
    /// Executa o próximo middleware e intercepta qualquer exceção não tratada.
    /// </summary>
    /// <param name="context">Contexto HTTP da requisição atual.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Constrói o payload <c>ProblemDetails</c> e escreve a resposta HTTP.
    /// </summary>
    /// <param name="context">Contexto HTTP da requisição atual.</param>
    /// <param name="exception">Exceção capturada pelo bloco catch.</param>
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Loga a exceção com stack trace completa para facilitar diagnóstico
        logger.LogError(exception, "Exceção não tratada: {Message}", exception.Message);

        // Switch expression para mapear tipo de exceção → ProblemDetails adequado
        var problem = exception switch
        {
            NotFoundException => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Recurso não encontrado",
                Detail = exception.Message,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
            },
            BusinessRuleException => new ProblemDetails
            {
                Status = StatusCodes.Status422UnprocessableEntity,
                Title = "Regra de negócio violada",
                Detail = exception.Message,
                Type = "https://tools.ietf.org/html/rfc4918#section-11.2"
            },
            // ValidationException carrega um dicionário de erros por campo
            AppValidationException validationEx => new ValidationProblemDetails(validationEx.Errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Erros de validação",
                Detail = exception.Message,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            },
            // Fallback genérico — não expõe detalhes internos ao cliente por segurança
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Erro interno do servidor",
                Detail = "Ocorreu um erro inesperado. Tente novamente mais tarde.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            }
        };

        // Inclui o path da requisição para facilitar o diagnóstico no log
        problem.Instance = context.Request.Path;
        context.Response.StatusCode = problem.Status!.Value;
        context.Response.ContentType = "application/problem+json";

        // Serializa com camelCase para conformidade com a convenção JSON REST
        var json = JsonSerializer.Serialize(problem, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
