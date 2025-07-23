namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest;

using System;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.LogNS;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Core.Domain.Entities.LogNS;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.General;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.LogsNS;
using IAVH.BioTablero.CM.WebApi.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

using Serilog;

using Swashbuckle.AspNetCore.Filters;

using static IAVH.BioTablero.CM.Core.Domain.Enums.LogEnums;

/// <summary>
/// Logs controller
/// </summary>
/// <param name="webTools">General web tools</param>
/// <param name="entityService">Entity service</param>
/// <param name="logger">Logging API</param>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class LogsController(IWebTools webTools,
    ILogService entityService,
    ILogger logger) : ODataController
{
    /// <summary>
    /// Get entity
    /// </summary>
    /// <param name="id">Entity identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Selected entity data</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(LogDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var response = await entityService.GetItem(id, ct);

        // TODO: delete this after ticket lib-230
        if (response.Success)
        {
            logger
                .ForContext("CustomRecord", true)
                .ForContext("Type", (int)LogType.Read)
                .Information("Get log: {@id}", id);
        }

        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Get entities (paginated)
    /// </summary>
    /// <param name="queryOptions">OData query options</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Entities list from parameters</returns>
    [HttpGet]
    [ProducesResponseType(typeof(LogOdataResponseExample), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LogOdataResponseExample))]
    public async Task<IActionResult> Get(ODataQueryOptions<LogEntity> queryOptions, CancellationToken ct)
    {
        var response = await entityService.GetList(queryOptions, ct);

        // TODO: delete this after ticket lib-230
        if (response.Success)
        {
            logger
                .ForContext("CustomRecord", true)
                .ForContext("Type", (int)LogType.Read)
                .Information("Get logs: {@queryOptions}", queryOptions.RawValues);
        }

        return webTools.CustomResponse(response);
    }
}
