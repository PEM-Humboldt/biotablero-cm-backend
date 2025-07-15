namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest;

using System;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Core.Entities.LogNS;
using IAVH.BioTablero.CM.WebApi.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

/// <summary>
/// Logs controller
/// </summary>
/// <param name="webTools">General web tools</param>
/// <param name="entityService">Entity service</param>
[ApiController]
[Route("[controller]")]
public class LogsController(IWebTools webTools,
    ILogService entityService) : ODataController
{
    /// <summary>
    /// Get entity
    /// </summary>
    /// <param name="id">Entity identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Selected entity data</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var response = await entityService.Get(id, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Get entities (paginated)
    /// </summary>
    /// <param name="queryOptions">OData query options</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Entities list from parameters</returns>
    [HttpGet]
    public async Task<IActionResult> Get(ODataQueryOptions<LogEntity> queryOptions, CancellationToken ct)
    {
        var response = await entityService.GetList(queryOptions, ct);
        return webTools.CustomResponse(response);
    }
}
