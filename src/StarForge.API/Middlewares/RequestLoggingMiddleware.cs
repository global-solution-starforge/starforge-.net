using System.Diagnostics;

namespace StarForge.API.Middlewares;

/// <summary>
/// Middleware de logging de requisições HTTP.
/// Registra método, path, status code e tempo de resposta de cada requisição.
/// </summary>
/// <remarks>
/// Registrado APÓS <c>ExceptionHandlingMiddleware</c> no pipeline, para que erros
/// já tratados também sejam logados com seu status code final (ex: 404, 422).
/// A medição usa <see cref="Stopwatch"/> de alta precisão (não <c>DateTime.Now</c>).
/// </remarks>
/// <param name="next">Próximo middleware no pipeline ASP.NET Core.</param>
/// <param name="logger">Logger injetado — grava no provider configurado (console, arquivo etc.).</param>
public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    /// <summary>
    /// Executa o próximo middleware medindo o tempo de resposta e registrando no log.
    /// </summary>
    /// <param name="context">Contexto HTTP da requisição atual.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        // Inicia o cronômetro antes de invocar o restante do pipeline
        var sw = Stopwatch.StartNew();
        await next(context);
        sw.Stop();

        // Loga uma linha por requisição: método, path, status e latência
        logger.LogInformation("{Method} {Path} → {StatusCode} em {ElapsedMs}ms",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            sw.ElapsedMilliseconds);
    }
}
