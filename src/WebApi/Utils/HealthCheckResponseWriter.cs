namespace IAVH.BioTablero.CM.WebApi.Utils;

using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

/// <summary>
/// Custom health check response writer.
/// </summary>
public static class HealthCheckResponseWriter
{
    /// <summary>
    /// Response writer function.
    /// </summary>
    public static readonly Func<HttpContext, HealthReport, Task> ResponseWriter =
        async (context, report) =>
        {
            context.Response.ContentType = "application/json";

            var response = new
            {
                status = report.Status.ToString(),
                results = report.Entries.ToDictionary(
                    e => e.Key,
                    e => e.Value.Status.ToString()),
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions));
        };

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
    };
}
