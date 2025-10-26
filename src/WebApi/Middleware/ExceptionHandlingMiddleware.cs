namespace IAVH.BioTablero.CM.WebApi.Middleware;

using System;
using System.Net;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Utils;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Global exception handling middleware.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger logger;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="next">Next middleware in pipeline.</param>
    /// <param name="logger">Logger instance.</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger logger)
    {
        this.next = next;
        this.logger = logger;
    }

    /// <summary>
    /// Invoke middleware.
    /// </summary>
    /// <param name="context">HTTP context.</param>
    /// <returns>Task.</returns>
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
    /// Handle exception and return appropriate response.
    /// </summary>
    /// <param name="context">HTTP context.</param>
    /// <param name="exception">Exception to handle.</param>
    /// <returns>Task.</returns>
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var customResponse = exception switch
        {
            OperationCanceledException => new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.RequestTimeout,
                Message = "Operation was cancelled",
            },
            InvalidOperationException ex => new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Invalid operation",
            },
            ArgumentException ex => new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Invalid argument",
            },
            TimeoutException ex => new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.RequestTimeout,
                Message = "Request timeout",
            },
            DbUpdateException ex => new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "Database error",
            },
            UnauthorizedAccessException ex => new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Message = "Unauthorized access",
            },
            NotImplementedException ex => new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.NotImplemented,
                Message = "Feature not implemented",
            },
            _ => new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "An unexpected error occurred",
            },
        };

        // Log the exception with appropriate level
        var logLevel = GetLogLevel(exception);
        logger.AddLog(LogType.System, "Exception occurred: {ExceptionType} - {Message}", new { ExceptionType = exception.GetType().Name, Message = exception.Message }, logLevel);

        response.StatusCode = (int)customResponse.StatusCode;
        await response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(customResponse));
    }

    /// <summary>
    /// Get appropriate log level based on exception type.
    /// </summary>
    /// <param name="exception">Exception to analyze.</param>
    /// <returns>Log level.</returns>
    private static Serilog.Events.LogEventLevel GetLogLevel(Exception exception) => exception switch
    {
        OperationCanceledException => Serilog.Events.LogEventLevel.Information,
        ArgumentException => Serilog.Events.LogEventLevel.Warning,
        InvalidOperationException => Serilog.Events.LogEventLevel.Warning,
        TimeoutException => Serilog.Events.LogEventLevel.Warning,
        DbUpdateException => Serilog.Events.LogEventLevel.Error,
        UnauthorizedAccessException => Serilog.Events.LogEventLevel.Warning,
        NotImplementedException => Serilog.Events.LogEventLevel.Warning,
        _ => Serilog.Events.LogEventLevel.Error,
    };
}
