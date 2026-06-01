namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Reports;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Reports;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Statistics;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Reports;
using IAVH.BioTablero.CM.WebApi.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// General statistics controller.
/// </summary>
/// <param name="webTools">General web tools.</param>
/// <param name="generalStatsService">General statistics service.</param>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[ApiConventionType(typeof(CustomApiConventions))]
public class GeneralStatsController(
    IWebTools webTools,
    IGeneralStatsService generalStatsService) : ControllerBase
{
    /// <summary>
    /// Get general statistics for community monitoring.
    /// </summary>
    /// <param name="departmentId">Department identifier (optional).</param>
    /// <param name="initiativeId">Initiative identifier (optional).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>General statistics data including initiatives, users, join requests, and recent activity.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(GeneralStatsDto), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GeneralStatisticsResponseExample))]
    public async Task<IActionResult> Get(int? departmentId = null, int? initiativeId = null, CancellationToken ct = default)
    {
        var response = await generalStatsService.GetStatsAsync(departmentId, initiativeId, ct);
        return webTools.CustomResponse(response);
    }
}
