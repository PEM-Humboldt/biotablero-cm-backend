namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Initiatives;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Storage;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;
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
/// Initiatives controller.
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
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeOdataResponseExample))]
    public async Task<IActionResult> GetOdataList(ODataQueryOptions<Initiative> queryOptions, CancellationToken ct)
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
    [HttpPut]
    [Consumes("application/json")]
    [Authorize(Roles = IamConstants.RoleModuleAdmin)]
    [SwaggerRequestExample(typeof(InitiativeDto), typeof(InitiativeAddRequestExample))]
    public async Task<IActionResult> Put([FromBody] InitiativeDto requestData, CancellationToken ct)
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
    [HttpPost("{id}")]
    [Consumes("application/json")]
    [Authorize(Roles = IamConstants.RoleModuleAdmin)]
    [SwaggerRequestExample(typeof(InitiativeDto), typeof(InitiativeEditRequestExample))]
    public async Task<IActionResult> Post(int id, [FromBody] InitiativeDto requestData, CancellationToken ct)
    {
        var response = await entityService.UpdateAsync(id, requestData, ct);
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
