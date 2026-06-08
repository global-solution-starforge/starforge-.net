using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using StarForge.Application.Exceptions;
using AppValidationException = StarForge.Application.Exceptions.ValidationException;

namespace StarForge.API.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        logger.LogError(exception, "Exceção não tratada: {Message}", exception.Message);

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
            AppValidationException validationEx => new ValidationProblemDetails(validationEx.Errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Erros de validação",
                Detail = exception.Message,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            },
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Erro interno do servidor",
                Detail = "Ocorreu um erro inesperado. Tente novamente mais tarde.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            }
        };

        problem.Instance = context.Request.Path;
        context.Response.StatusCode = problem.Status!.Value;
        context.Response.ContentType = "application/problem+json";

        var json = JsonSerializer.Serialize(problem, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
