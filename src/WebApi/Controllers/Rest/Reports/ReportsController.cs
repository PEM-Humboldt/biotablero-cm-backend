namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Reports;

using System;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Reports;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Reports;
using IAVH.BioTablero.CM.Core.Domain.Entities.Reports;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Reports;
using IAVH.BioTablero.CM.WebApi.Interfaces;
using IAVH.BioTablero.CM.WebApi.Utils;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

/// <summary>
/// Reports controller.
/// </summary>
/// <param name="webTools">General web tools.</param>
/// <param name="entityService">Entity service.</param>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[ApiConventionType(typeof(CustomApiConventions))]
public class ReportsController(
    IWebTools webTools,
    IReportDataService entityService) : ControllerBase
{
    /// <summary>
    /// Get entity.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Selected entity data.</returns>
    [HttpGet("{id}")]
    [Authorize(Roles = IamConstants.RoleModuleAdmin)]
    [OpenApiResponse(StatusCodes.Status200OK, typeof(ReportDataResponseExample))]
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
    [Authorize(Roles = IamConstants.RoleModuleAdmin)]
    [OpenApiResponse(StatusCodes.Status200OK, typeof(ReportDataOdataResponseExample))]
    public async Task<IActionResult> GetOdataList(ODataQueryOptions<ReportData> queryOptions, CancellationToken ct)
    {
        var response = await entityService.GetListAsync(queryOptions, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Add entity.
    /// </summary>
    /// <param name="requestData">Request data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Added entity data.</returns>
    [HttpPost]
    [Consumes("application/json")]
    [Authorize]
    [OpenApiRequest(typeof(ReportDataAddRequestExample))]
    [OpenApiResponse(StatusCodes.Status200OK, typeof(ReportDataResponseExample))]
    public async Task<IActionResult> Post([FromBody] ReportDataDto requestData, CancellationToken ct)
    {
        var userName = HttpContext.GetUserName();
        ArgumentException.ThrowIfNullOrEmpty(userName);

        requestData.UserName = userName;
        var response = await entityService.AddAsync(requestData, ct);
        return webTools.CustomResponse(response);
    }
}
