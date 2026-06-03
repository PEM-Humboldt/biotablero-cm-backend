namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Reports;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Reports;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Reports;
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
    /// Get general statistics.
    /// </summary>
    /// <param name="departmentId">Department identifier (optional).</param>
    /// <param name="initiativeId">Initiative identifier (optional).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>General statistics data.</returns>
    [HttpGet("General")]
    [ProducesResponseType(typeof(GeneralStatsDto), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GeneralStatsResponseExample))]
    public async Task<IActionResult> GetGeneralStats(int? departmentId = null, int? initiativeId = null, CancellationToken ct = default)
    {
        var response = await generalStatsService.GetGeneralStatsAsync(departmentId, initiativeId, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Get ecosystems statistics.
    /// </summary>
    /// <param name="departmentId">Department identifier (optional).</param>
    /// <param name="initiativeId">Initiative identifier (optional).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Ecosystems statistics data.</returns>
    [HttpGet("Ecosystems")]
    [ProducesResponseType(typeof(EcosystemsStatsDto), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EcosystemsStatsResponseExample))]
    public async Task<IActionResult> GetEcosystemsStats(int? departmentId = null, int? initiativeId = null, CancellationToken ct = default)
    {
        var response = await generalStatsService.GetEcosystemsStatsAsync(departmentId, initiativeId, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Get demographic statistics.
    /// </summary>
    /// <param name="departmentId">Department identifier (optional).</param>
    /// <param name="initiativeId">Initiative identifier (optional).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Demographic statistics data.</returns>
    [HttpGet("Demographic")]
    [ProducesResponseType(typeof(DemographicStatsDto), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DemographicStatsResponseExample))]
    public async Task<IActionResult> GetDemographicStats(int? departmentId = null, int? initiativeId = null, CancellationToken ct = default)
    {
        var response = await generalStatsService.GetDemographicStats(departmentId, initiativeId, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Get indicators statistics.
    /// </summary>
    /// <param name="departmentId">Department identifier (optional).</param>
    /// <param name="initiativeId">Initiative identifier (optional).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Indicators statistics data.</returns>
    [HttpGet("Indicators")]
    [ProducesResponseType(typeof(IndicatorsStatsDto), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(IndicatorsStatsResponseExample))]
    public async Task<IActionResult> GetIndicatorsStats(int? departmentId = null, int? initiativeId = null, CancellationToken ct = default)
    {
        var response = await generalStatsService.GetIndicatorsStats(departmentId, initiativeId, ct);
        return webTools.CustomResponse(response);
    }
}
