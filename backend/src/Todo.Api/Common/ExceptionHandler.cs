using Microsoft.AspNetCore.Mvc;

namespace Todo.Api.Common;

public class ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandler> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            var (statusCode, title) = ex switch
            {
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
                ArgumentException => (StatusCodes.Status400BadRequest, "Invalid request"),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };

            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = ex.Message,
                Instance = context.Request.Path
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}