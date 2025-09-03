namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Core.Domain.Entities.Logging;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Logging;
using IAVH.BioTablero.CM.WebApi.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Logs controller.
/// </summary>
/// <param name="webTools">General web tools.</param>
/// <param name="entityService">Entity service.</param>
[Authorize(Roles = IamConstants.RoleModuleAdmin)]
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[ApiConventionType(typeof(CustomApiConventions))]
public class LogsController(IWebTools webTools,
    ILogService entityService) : ODataController
{
    /// <summary>
    /// Get entity.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Selected entity data.</returns>
    [HttpGet("{id}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LogResponseExample))]
    public async Task<IActionResult> GetItem(Guid id, CancellationToken ct)
    {
        var response = await entityService.GetItemAsync(id, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Get entities (paginated).
    /// </summary>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities list from parameters.</returns>
    [HttpGet]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LogOdataResponseExample))]
    public async Task<IActionResult> GetOdataList(ODataQueryOptions<LogEntity> queryOptions, CancellationToken ct)
    {
        var response = await entityService.GetListAsync(queryOptions, ct);
        return webTools.CustomResponse(response);
    }
}
