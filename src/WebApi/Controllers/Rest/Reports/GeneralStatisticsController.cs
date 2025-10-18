namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Reports;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Reports;
using IAVH.BioTablero.CM.WebApi.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// General statistics controller for community monitoring.
/// </summary>
/// <param name="webTools">General web tools.</param>
/// <param name="generalStatisticsService">General statistics service.</param>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[ApiConventionType(typeof(CustomApiConventions))]
public class GeneralStatisticsController(
    IWebTools webTools,
    IGeneralStatisticsService generalStatisticsService) : ControllerBase
{
    /// <summary>
    /// Get general statistics for community monitoring.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>General statistics data including initiatives, users, join requests, and recent activity.</returns>
    [HttpGet]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GeneralStatisticsResponseExample))]
    public async Task<IActionResult> GetGeneralStatistics(CancellationToken ct = default)
    {
        var response = await generalStatisticsService.GetGeneralStatisticsAsync(ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Get general statistics filtered by department.
    /// </summary>
    /// <param name="departmentId">Department identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>General statistics data for the specified department.</returns>
    [HttpGet("department/{departmentId}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GeneralStatisticsResponseExample))]
    public async Task<IActionResult> GetGeneralStatisticsByDepartment(int departmentId, CancellationToken ct = default)
    {
        var response = await generalStatisticsService.GetGeneralStatisticsByDepartmentAsync(departmentId, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Get general statistics filtered by initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>General statistics data for the specified initiative.</returns>
    [HttpGet("initiative/{initiativeId}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GeneralStatisticsResponseExample))]
    public async Task<IActionResult> GetGeneralStatisticsByInitiative(int initiativeId, CancellationToken ct = default)
    {
        var response = await generalStatisticsService.GetGeneralStatisticsByInitiativeAsync(initiativeId, ct);
        return webTools.CustomResponse(response);
    }
}
