using System.Diagnostics;

namespace StarForge.API.Middlewares;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        await next(context);
        sw.Stop();

        logger.LogInformation("{Method} {Path} → {StatusCode} em {ElapsedMs}ms",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            sw.ElapsedMilliseconds);
    }
}
