namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Notifications;

using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.Services.Notifications;
using IAVH.BioTablero.CM.Core.Domain.Entities.Notifications;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Notification;
using IAVH.BioTablero.CM.WebApi.Interfaces;
using IAVH.BioTablero.CM.WebApi.Utils;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Notification controller.
/// </summary>
/// <param name="webTools">General web tools.</param>
/// <param name="entityService">Entity service.</param>
/// <param name="sseDispatcher">SSE dispatcher service.</param>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[Authorize]
[ApiConventionType(typeof(CustomApiConventions))]
public class NotificationController(
    IWebTools webTools,
    INotificationService entityService,
    ISseNotificationDispatcher sseDispatcher) : ODataController
{
    private readonly JsonSerializerOptions jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    /// <summary>
    /// Get entity.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Selected entity data.</returns>
    [HttpGet("{id}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(NotificationResponseExample))]
    public async Task<IActionResult> GetItem(int id, CancellationToken ct)
    {
        var response = await entityService.GetItemAsync(id, HttpContext.GetUserName(), ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Get my not read total notifications.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Total not read notifications.</returns>
    [HttpGet("TotalNotRead")]
    public async Task<IActionResult> GetTotalNotRead(CancellationToken ct)
    {
        var response = await entityService.GetTotalNotReadByUserNameAsync(HttpContext.GetUserName(), ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Get my notifications (paginated).
    /// </summary>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities list from parameters.</returns>
    [HttpGet]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(NotificationOdataResponseExample))]
    public async Task<IActionResult> GetOdataList(ODataQueryOptions<Notification> queryOptions, CancellationToken ct)
    {
        var response = await entityService.GetByUserNameAsync(HttpContext.GetUserName(), queryOptions, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Subscribe to Server-Sent Events (SSE) for notifications.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A stream of notification events.</returns>
    [HttpGet("Stream")]
    public async Task Stream(CancellationToken ct)
    {
        var userName = HttpContext.GetUserName();
        var connectionId = Guid.NewGuid();

        Response.Headers.Append("Content-Type", "text/event-stream");
        Response.Headers.Append("Cache-Control", "no-cache");
        Response.Headers.Append("Connection", "keep-alive");

        var channelReader = sseDispatcher.Subscribe(userName, connectionId);

        try
        {
            while (await channelReader.WaitToReadAsync(ct))
            {
                while (channelReader.TryRead(out var notification))
                {
                    var payload = JsonSerializer.Serialize(notification, jsonSerializerOptions);
                    await Response.WriteAsync($"data: {payload}\n\n", ct);
                    await Response.Body.FlushAsync(ct);
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Client disconnected naturally
        }
        finally
        {
            sseDispatcher.Unsubscribe(userName, connectionId);
        }
    }
}
