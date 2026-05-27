namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Reports;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.Services.Statistics;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Reports;
using IAVH.BioTablero.CM.WebApi.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Initiative statistics controller.
/// </summary>
/// <param name="webTools">General web tools.</param>
/// <param name="initiativeStatsService">Initiative statistics service.</param>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[ApiConventionType(typeof(CustomApiConventions))]
public class InitiativeStatsController(
    IWebTools webTools,
    IInitiativeStatsService initiativeStatsService) : ControllerBase
{
    /// <summary>
    /// Get initiative statistics.
    /// </summary>
    /// <param name="id">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Initiative statistics.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(InitiativeStatsResponseExample), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeStatsResponseExample))]
    public async Task<IActionResult> Get(int id, CancellationToken ct = default)
    {
        var response = await initiativeStatsService.GetStats(id, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Get monitoring events data.
    /// </summary>
    /// <param name="id">Initiative identifier.</param>
    /// <param name="year">Year filter (optional).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Monitoring Events data.</returns>
    [HttpGet("GetMonitoringEvents/{id}")]
    [ProducesResponseType(typeof(MonitoringEventsResponseExample), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MonitoringEventsResponseExample))]
    public async Task<IActionResult> GetMonitoringEvents(int id, int? year, CancellationToken ct = default)
    {
        var response = await initiativeStatsService.GetMonitoringEvents(id, year, ct);
        return webTools.CustomResponse(response);
    }
}
