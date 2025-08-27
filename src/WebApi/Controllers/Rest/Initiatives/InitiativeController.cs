namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Initiatives;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Storage;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.General;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;
using IAVH.BioTablero.CM.WebApi.Interfaces;

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
    [ProducesResponseType(typeof(InitiativeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeResponseExample))]
    public async Task<IActionResult> Get(int id, CancellationToken ct)
    {
        var response = await entityService.GetItem(id, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Get entities (paginated).
    /// </summary>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities list from parameters.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(InitiativeOdataResponseExample), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeOdataResponseExample))]
    public async Task<IActionResult> Get(ODataQueryOptions<Initiative> queryOptions, CancellationToken ct)
    {
        var response = await entityService.GetList(queryOptions, ct);
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
        var response = await entityService.GetPolygon(id, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Add entity.
    /// </summary>
    /// <param name="requestData">Request data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Added entity data.</returns>
    [HttpPut]
    [Consumes("application/json")]

    // [Authorize(Roles = IamConstants.RoleModuleAdmin)]
    [SwaggerRequestExample(typeof(InitiativeDto), typeof(InitiativeAddRequestExample))]
    [ProducesResponseType(typeof(InitiativeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeResponseExample))]
    public async Task<IActionResult> Put([FromBody] InitiativeDto requestData, CancellationToken ct)
    {
        var response = await entityService.Add(requestData, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Edit entity.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="requestData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Updated entity data.</returns>
    [HttpPost("{id}")]
    [Consumes("application/json")]
    [Authorize(Roles = IamConstants.RoleModuleAdmin)]
    [SwaggerRequestExample(typeof(InitiativeDto), typeof(InitiativeEditRequestExample))]
    [ProducesResponseType(typeof(InitiativeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeResponseExample))]
    public async Task<IActionResult> Post(int id, [FromBody] InitiativeDto requestData, CancellationToken ct)
    {
        var response = await entityService.Update(id, requestData, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Update entity polygon.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="request">Polygon request.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Updated entity data.</returns>
    [HttpPost("Polygon/{id}")]
    [Consumes("text/json")]
    [Authorize(Roles = IamConstants.RoleModuleAdmin)]
    [SwaggerRequestExample(typeof(string), typeof(InitiativePolygonEditRequestExample))]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeResponseExample))]
    public async Task<IActionResult> UpdatePolygon(int id, [FromBody] string request, CancellationToken ct)
    {
        var response = await entityService.UpdatePolygon(id, request, ct);
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
        var response = await entityService.UploadImage(id, new FormFileAdapter(formFile), InitiativeImageType.Image, ct);
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
        var response = await entityService.UploadImage(id, new FormFileAdapter(formFile), InitiativeImageType.Banner, ct);
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
    [ProducesResponseType(typeof(InitiativeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeResponseExample))]
    public async Task<IActionResult> Enable(int id, CancellationToken ct)
    {
        var response = await entityService.Disable(id, false, ct);
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
    [ProducesResponseType(typeof(InitiativeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeResponseExample))]
    public async Task<IActionResult> Disable(int id, CancellationToken ct)
    {
        var response = await entityService.Disable(id, true, ct);
        return webTools.CustomResponse(response);
    }
}
