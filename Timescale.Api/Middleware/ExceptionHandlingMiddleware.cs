using System.Text.Json;
using Timecale.Application.Exceptions;

namespace Timescale.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await HandleExceptionAsync(
                context,
                StatusCodes.Status400BadRequest,
                ex.Message);
        }
        catch (FormatException ex)
        {
            await HandleExceptionAsync(
                context,
                StatusCodes.Status400BadRequest,
                ex.Message);
        }
        catch (Exception)
        {
            await HandleExceptionAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "Произошла внутренняя ошибка сервера");
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        int statusCode,
        string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            error = message
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}