namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Notifications;

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
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[Authorize]
[ApiConventionType(typeof(CustomApiConventions))]
public class NotificationController(
    IWebTools webTools,
    INotificationService entityService) : ODataController
{
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
    /// Get my unreaded total notifications.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Total unreaded notifications.</returns>
    [HttpGet("TotalUnreaded")]
    public async Task<IActionResult> GetTotalUnreaded(CancellationToken ct)
    {
        var response = await entityService.GetTotalUnreadedByUserNameAsync(HttpContext.GetUserName(), ct);
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
}
