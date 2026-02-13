using System.Net;
using System.Text.Json;
using FluentValidation;
using TeamFlow.Application.Common.Exceptions;

namespace TeamFlow.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = context.Response;

        switch (exception)
        {
            case ValidationException validationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;

                var errors = validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray());

                await response.WriteAsync(JsonSerializer.Serialize(new
                {
                    title = "Validation failed",
                    status = response.StatusCode,
                    errors
                }));
                break;

            case NotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                await response.WriteAsync(JsonSerializer.Serialize(new
                {
                    title = exception.Message,
                    status = response.StatusCode
                }));
                break;

            case UnauthorizedException:
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await response.WriteAsync(JsonSerializer.Serialize(new
                {
                    title = exception.Message,
                    status = response.StatusCode
                }));
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await response.WriteAsync(JsonSerializer.Serialize(new
                {
                    title = "Internal Server Error",
                    status = response.StatusCode
                }));
                break;
        }
    }
}
