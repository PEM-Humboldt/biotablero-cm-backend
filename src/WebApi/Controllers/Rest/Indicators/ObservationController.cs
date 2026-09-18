namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Indicators;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Indicators;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Storage;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Observation;
using IAVH.BioTablero.CM.WebApi.Interfaces;
using IAVH.BioTablero.CM.WebApi.Utils;
using IAVH.BioTablero.CM.WebApi.Utils.Requests.Indicators;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

/// <summary>
/// Observation controller.
/// </summary>
/// <param name="webTools">General web tools.</param>
/// <param name="entityService">Entity service.</param>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[ApiConventionType(typeof(CustomApiConventions))]
public class ObservationController(
    IWebTools webTools,
    IObservationService entityService) : ODataController
{
    /// <summary>
    /// Get entity.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Selected entity data.</returns>
    [HttpGet("{id}")]
    [OpenApiResponse(StatusCodes.Status200OK, typeof(ObservationResponseExample))]
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
    [OpenApiResponse(StatusCodes.Status200OK, typeof(ObservationOdataResponseExample))]
    public async Task<IActionResult> GetOdataList(ODataQueryOptions<Observation> queryOptions, CancellationToken ct)
    {
        var response = await entityService.GetListAsync(queryOptions, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Get entities by Initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities list from parameters.</returns>
    [HttpGet("GetByInitiative/{initiativeId}")]
    [OpenApiResponse(StatusCodes.Status200OK, typeof(ObservationListResponseExample))]
    public async Task<IActionResult> GetListByInitiative(int initiativeId, CancellationToken ct)
    {
        var response = await entityService.GetByInitiativeAsync(initiativeId, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Edit entity.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="requestData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Updated entity data.</returns>
    [HttpPut("{id}")]
    [Consumes("application/json")]
    [Authorize(Roles = IamConstants.RoleModuleAdmin)]
    [OpenApiRequest(typeof(ObservationEditRequestExample))]
    [OpenApiResponse(StatusCodes.Status200OK, typeof(ObservationResponseExample))]
    public async Task<IActionResult> Put(int id, [FromBody] ObservationDto requestData, CancellationToken ct)
    {
        var response = await entityService.UpdateAsync(id, requestData, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Import indicators with spreadsheet.
    /// </summary>
    /// <param name="requestData">Indicators request data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    [Authorize(Roles = IamConstants.RoleModuleAdmin)]
    [HttpPost("Import")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Import([FromForm] IndicatorsImportFileRequest requestData, CancellationToken ct)
    {
        var requestDataDto = new ObservationsImportFileDto()
        {
            Id = requestData.Id,
            InitiativeId = requestData.InitiativeId,
            DoNotModifyDatabase = requestData.DoNotModifyDatabase,
        };

        var response = await entityService.ImportObservationsAsync(HttpContext.GetUserName(), requestDataDto, new FormFileAdapter(requestData.File), ct);
        return webTools.CustomResponse(response);
    }
}
