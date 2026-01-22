namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Resources;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Resources;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Resources;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Storage;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.ResourceFile;
using IAVH.BioTablero.CM.WebApi.Interfaces;
using IAVH.BioTablero.CM.WebApi.Utils;
using IAVH.BioTablero.CM.WebApi.Utils.Requests.ResourceFile;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Resource File controller.
/// </summary>
/// <param name="webTools">General web tools.</param>
/// <param name="entityService">Entity service.</param>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[ApiConventionType(typeof(CustomApiConventions))]
public class ResourceFileController(
    IWebTools webTools,
    IResourceFileService entityService) : ControllerBase
{
    /// <summary>
    /// Get entity.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Selected entity data.</returns>
    [HttpGet("{id}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ResourceFileResponseExample))]
    public async Task<IActionResult> GetItem(int id, CancellationToken ct)
    {
        var response = await entityService.GetItemAsync(id, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Get entities by Resource.
    /// </summary>
    /// <param name="resourceId">Resource identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities list from parameters.</returns>
    [HttpGet("GetByResource/{resourceId}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ResourceFileListResponseExample))]
    public async Task<IActionResult> GetListByResource(int resourceId, CancellationToken ct)
    {
        var response = await entityService.GetByResourceAsync(resourceId, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Add entity.
    /// </summary>
    /// <param name="requestData">Request data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Added entity data.</returns>
    [HttpPut]
    [Authorize]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ResourceFileResponseExample))]
    public async Task<IActionResult> Put([FromForm] ResourceFileAddRequest requestData, CancellationToken ct)
    {
        var requestDataDto = new ResourceFileDto()
        {
            ResourceId = requestData.ResourceId,
            Name = requestData.Name,
        };

        var response = await entityService.AddAsync(HttpContext.GetUserName(), requestDataDto, new FormFileAdapter(requestData.File), ct);
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
    [Authorize]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ResourceFileResponseExample))]
    public async Task<IActionResult> Post(int id, [FromForm] ResourceFileEditRequest requestData, CancellationToken ct)
    {
        var requestDataDto = new ResourceFileDto()
        {
            Name = requestData.Name,
        };

        var response = await entityService.UpdateAsync(id, HttpContext.GetUserName(), requestDataDto, new FormFileAdapter(requestData.File), ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Delete entity.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var response = await entityService.DeleteAsync(id, HttpContext.GetUserName(), ct);
        return webTools.CustomResponse(response);
    }
}
