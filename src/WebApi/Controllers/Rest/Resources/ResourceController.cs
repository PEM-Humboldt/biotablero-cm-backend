namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Resources;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Resources;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Resources;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Resource;
using IAVH.BioTablero.CM.WebApi.Interfaces;
using IAVH.BioTablero.CM.WebApi.Utils;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Resource controller.
/// </summary>
/// <param name="webTools">General web tools.</param>
/// <param name="entityService">Entity service.</param>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[ApiConventionType(typeof(CustomApiConventions))]
public class ResourceController(
    IWebTools webTools,
    IResourceService entityService) : ODataController
{
    /// <summary>
    /// Get entity.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Selected entity data.</returns>
    [HttpGet("{id}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ResourceResponseExample))]
    public async Task<IActionResult> GetItem(int id, CancellationToken ct)
    {
        var response = await entityService.GetItemAsync(id, HttpContext.GetUserName(), ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Get entities by Initiative (paginated).
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities list from parameters.</returns>
    [HttpGet("GetByInitiative/{initiativeId}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ResourceOdataResponseExample))]
    public async Task<IActionResult> GetOdataListByInitiative(int initiativeId, ODataQueryOptions<Resource> queryOptions, CancellationToken ct)
    {
        var response = await entityService.GetByInitiativeAsync(initiativeId, HttpContext.GetUserName(), queryOptions, ct);
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
    [SwaggerRequestExample(typeof(ResourceDto), typeof(ResourceAddRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ResourceResponseExample))]
    public async Task<IActionResult> Post([FromBody] ResourceDto requestData, CancellationToken ct)
    {
        var response = await entityService.AddAsync(HttpContext.GetUserName(), requestData, ct);
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
    [SwaggerRequestExample(typeof(ResourceDto), typeof(ResourceEditRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ResourceResponseExample))]
    public async Task<IActionResult> Put(int id, [FromBody] ResourceDto requestData, CancellationToken ct)
    {
        var response = await entityService.UpdateAsync(id, HttpContext.GetUserName(), requestData, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Like action.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    [HttpPost("Like/{id}")]
    [Authorize]
    public async Task<IActionResult> Like(int id, CancellationToken ct)
    {
        var requestData = new ResourceLikeDto()
        {
            ResourceId = id,
            UserName = HttpContext.GetUserName(),
        };

        var response = await entityService.LikeActionAsync(requestData, ct);
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
