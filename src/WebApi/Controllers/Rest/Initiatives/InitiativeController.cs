namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Initiatives;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Storage;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;
using IAVH.BioTablero.CM.WebApi.Interfaces;
using IAVH.BioTablero.CM.WebApi.Utils;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

using Swashbuckle.AspNetCore.Filters;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Initiative controller.
/// </summary>
/// <param name="webTools">General web tools.</param>
/// <param name="entityService">Entity service.</param>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[ApiConventionType(typeof(CustomApiConventions))]
public class InitiativeController(
    IWebTools webTools,
    IInitiativeService entityService) : ODataController
{
    /// <summary>
    /// Get entity.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Selected entity data.</returns>
    [HttpGet("{id}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeResponseExample))]
    public async Task<IActionResult> GetItem(int id, CancellationToken ct)
    {
        var response = await entityService.GetItemAsync(id, !string.IsNullOrEmpty(HttpContext.GetUserName()), ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Get entities (paginated).
    /// </summary>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities list from parameters.</returns>
    [HttpGet]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeOdataResponseExample))]
    public async Task<IActionResult> GetOdataList(ODataQueryOptions<Initiative> queryOptions, CancellationToken ct)
    {
        var response = await entityService.GetListAsync(queryOptions, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Get active initiatives with coordinates by location.
    /// </summary>
    /// <param name="locationId">Location identifier (optional). If null, returns all active initiatives.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of initiatives with coordinates.</returns>
    [HttpGet("GetByLocation")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeGeoDataResponseExample))]
    public async Task<IActionResult> GetListByLocation([FromQuery] int? locationId = null, CancellationToken ct = default)
    {
        var response = await entityService.GetByLocationAsync(locationId, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Get related initiatives.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities list from parameters.</returns>
    [HttpGet("Related")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeListResponseExample))]
    public async Task<IActionResult> GetListRelated(CancellationToken ct)
    {
        var response = await entityService.GetLastEntitiesAsync(ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Get entity polygon.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Polygon entity.</returns>
    [HttpGet("Polygon/{id}")]
    public async Task<IActionResult> GetPolygon(int id, CancellationToken ct)
    {
        var response = await entityService.GetPolygonAsync(id, ct);
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
    [Authorize(Roles = IamConstants.RoleModuleAdmin)]
    [SwaggerRequestExample(typeof(InitiativeDto), typeof(InitiativeAddRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeResponseExample))]
    public async Task<IActionResult> Post([FromBody] InitiativeDto requestData, CancellationToken ct)
    {
        var response = await entityService.AddAsync(requestData, ct);
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
    [Authorize]
    [SwaggerRequestExample(typeof(InitiativeDto), typeof(InitiativeEditRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeResponseExample))]
    public async Task<IActionResult> Put(int id, [FromBody] InitiativeDto requestData, CancellationToken ct)
    {
        var response = await entityService.UpdateAsync(id, HttpContext.GetUserName(), HttpContext.UserIsAdmin(), requestData, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Update entity polygon.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="request">Polygon request.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Updated entity data.</returns>
    [HttpPut("Polygon/{id}")]
    [Consumes("application/json")]
    [Authorize(Roles = IamConstants.RoleModuleAdmin)]
    [SwaggerRequestExample(typeof(string), typeof(InitiativePolygonEditRequestExample))]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeResponseExample))]
    public async Task<IActionResult> UpdatePolygon(int id, [FromBody] object request, CancellationToken ct)
    {
        var geoJsonString = System.Text.Json.JsonSerializer.Serialize(request);
        var response = await entityService.UpdatePolygonAsync(id, geoJsonString, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Upload entity image.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="formFile">Entity image.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Updated entity data.</returns>
    [HttpPost("UploadImage/{id}")]
    [Authorize(Roles = IamConstants.RoleModuleAdmin)]
    public async Task<IActionResult> UploadImage(int id, IFormFile formFile, CancellationToken ct)
    {
        var response = await entityService.UploadImageAsync(id, new FormFileAdapter(formFile), InitiativeImageType.Image, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Remove entity image.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    [HttpDelete("RemoveImage/{id}")]
    [Authorize(Roles = IamConstants.RoleModuleAdmin)]
    public async Task<IActionResult> RemoveImage(int id, CancellationToken ct)
    {
        var response = await entityService.RemoveImageAsync(id, InitiativeImageType.Image, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Upload entity banner.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="formFile">Entity banner.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Updated entity data.</returns>
    [HttpPost("UploadBanner/{id}")]
    [Authorize(Roles = IamConstants.RoleModuleAdmin)]
    public async Task<IActionResult> UploadBanner(int id, IFormFile formFile, CancellationToken ct)
    {
        var response = await entityService.UploadImageAsync(id, new FormFileAdapter(formFile), InitiativeImageType.Banner, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Remove entity banner.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    [HttpDelete("RemoveBanner/{id}")]
    [Authorize(Roles = IamConstants.RoleModuleAdmin)]
    public async Task<IActionResult> RemoveBanner(int id, CancellationToken ct)
    {
        var response = await entityService.RemoveImageAsync(id, InitiativeImageType.Banner, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Enable entity.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    [HttpPost("Enable/{id}")]
    [Authorize(Roles = IamConstants.RoleModuleAdmin)]
    public async Task<IActionResult> Enable(int id, CancellationToken ct)
    {
        var response = await entityService.EnableAsync(id, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Disable entity.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    [HttpDelete("Disable/{id}")]
    [Authorize(Roles = IamConstants.RoleModuleAdmin)]
    public async Task<IActionResult> Disable(int id, CancellationToken ct)
    {
        var response = await entityService.DisableAsync(id, ct);
        return webTools.CustomResponse(response);
    }
}
