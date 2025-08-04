namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Geo;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Geo;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.General;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Location;

using IAVH.BioTablero.CM.WebApi.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Location controller
/// </summary>
/// <param name="webTools">General web tools</param>
/// <param name="entityService">Entity service</param>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class LocationController(IWebTools webTools,
    ILocationService entityService) : ODataController
{
    /// <summary>
    /// Get entity
    /// </summary>
    /// <param name="id">Entity identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Selected entity data</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(LocationDto), StatusCodes.Status200OK)]
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
    [ProducesResponseType(typeof(LocationOdataResponseExample), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LocationOdataResponseExample))]
    public async Task<IActionResult> Get(ODataQueryOptions<Location> queryOptions, CancellationToken ct)
    {
        var response = await entityService.GetList(queryOptions, ct);
        return webTools.CustomResponse(response);
    }
}
