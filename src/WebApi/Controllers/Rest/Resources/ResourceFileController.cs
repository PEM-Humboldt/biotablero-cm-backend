namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Resources;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.Services.Resources;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.ResourceFile;
using IAVH.BioTablero.CM.WebApi.Interfaces;

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
}
