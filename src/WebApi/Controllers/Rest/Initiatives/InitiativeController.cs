namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Initiatives;

using System;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.General;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Logging;
using IAVH.BioTablero.CM.WebApi.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Initiatives controller
/// </summary>
/// <param name="webTools">General web tools</param>
/// <param name="entityService">Entity service</param>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class InitiativeController(IWebTools webTools,
    IInitiativeService entityService) : ODataController
{
    /// <summary>
    /// Get entity
    /// </summary>
    /// <param name="id">Entity identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Selected entity data</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(InitiativeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
    public async Task<IActionResult> Get(int id, CancellationToken ct)
    {
        var response = await entityService.GetItem(id, ct);
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
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeOdataResponseExample))]
    public async Task<IActionResult> Get(ODataQueryOptions<Initiative> queryOptions, CancellationToken ct)
    {
        var response = await entityService.GetList(queryOptions, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Add entity
    /// </summary>
    /// <param name="requestData">Request data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Added entity data</returns>
    [HttpPut]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(InitiativeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeAddResponseExample))]
    public async Task<IActionResult> Put([FromBody] InitiativeDto requestData, CancellationToken ct)
    {
        var response = await entityService.Add(requestData, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Edit entity
    /// </summary>
    /// <param name="id">Entity identifier</param>
    /// <param name="entityData">Entity data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Updated entity data</returns>
    [HttpPost("{id}")]
    [Consumes("application/json")]
    public Task<IActionResult> Post(int id, [FromBody] InitiativeDto entityData, CancellationToken ct) => throw new NotImplementedException();

    /// <summary>
    /// Disable entity
    /// </summary>
    /// <param name="id">Entity identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    [HttpDelete("{id}")]
    public Task<IActionResult> Disable(int id, CancellationToken ct) => throw new NotImplementedException();
}
