namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Indicators;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.Services.Indicators;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Indicator;
using IAVH.BioTablero.CM.WebApi.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Indicator controller.
/// </summary>
/// <param name="webTools">General web tools.</param>
/// <param name="entityService">Entity service.</param>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[ApiConventionType(typeof(CustomApiConventions))]
public class IndicatorController(
    IWebTools webTools,
    IIndicatorService entityService) : ODataController
{
    /// <summary>
    /// Get entity.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Selected entity data.</returns>
    [HttpGet("{id}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(IndicatorResponseExample))]
    public async Task<IActionResult> GetItem(int id, CancellationToken ct)
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
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(IndicatorOdataResponseExample))]
    public async Task<IActionResult> GetOdataList(ODataQueryOptions<Indicator> queryOptions, CancellationToken ct)
    {
        var response = await entityService.GetListAsync(queryOptions, ct);
        return webTools.CustomResponse(response);
    }
}
