namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.TerritoryStories;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Application.Interfaces.Services.TerritoryStory;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.TerritoryStory;
using IAVH.BioTablero.CM.WebApi.Interfaces;
using IAVH.BioTablero.CM.WebApi.Utils;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Territory Story controller.
/// </summary>
/// <param name="webTools">General web tools.</param>
/// <param name="entityService">Entity service.</param>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[ApiConventionType(typeof(CustomApiConventions))]
public class TerritoryStoryController(
    IWebTools webTools,
    ITerritoryStoryService entityService) : ODataController
{
    /// <summary>
    /// Get entity.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Selected entity data.</returns>
    [HttpGet("{id}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TerritoryStoryResponseExample))]
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
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TerritoryStoryOdataResponseExample))]
    public async Task<IActionResult> GetOdataList(ODataQueryOptions<TerritoryStory> queryOptions, CancellationToken ct)
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
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TerritoryStoryListResponseExample))]
    public async Task<IActionResult> GetListByInitiative(int initiativeId, CancellationToken ct)
    {
        var response = await entityService.GetByInitiativeAsync(initiativeId, ct);
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
    [Authorize]
    [SwaggerRequestExample(typeof(TerritoryStoryDto), typeof(TerritoryStoryAddRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TerritoryStoryResponseExample))]
    public async Task<IActionResult> Put([FromBody] TerritoryStoryDto requestData, CancellationToken ct)
    {
        requestData.AuthorUserName = HttpContext.GetUserName();
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
    [Authorize]
    [SwaggerRequestExample(typeof(TerritoryStoryDto), typeof(TerritoryStoryEditRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TerritoryStoryResponseExample))]
    public async Task<IActionResult> Post(int id, [FromBody] TerritoryStoryDto requestData, CancellationToken ct)
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
        var requestData = new TerritoryStoryLikeDto()
        {
            TerritoryStoryId = id,
            UserName = HttpContext.GetUserName(),
        };

        var response = await entityService.LikeActionAsync(requestData, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Featured content action.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    [HttpPost("FeaturedContent/{id}")]
    [Authorize]
    public async Task<IActionResult> FeaturedContent(int id, CancellationToken ct)
    {
        var response = await entityService.FeaturedContentActionAsync(id, HttpContext.GetUserName(), ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Enable entity.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    [HttpPost("Enable/{id}")]
    [Authorize]
    public async Task<IActionResult> Enable(int id, CancellationToken ct)
    {
        var response = await entityService.EnableAsync(id, HttpContext.GetUserName(), ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Disable entity.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    [HttpDelete("Disable/{id}")]
    [Authorize]
    public async Task<IActionResult> Disable(int id, CancellationToken ct)
    {
        var response = await entityService.DisableAsync(id, HttpContext.GetUserName(), ct);
        return webTools.CustomResponse(response);
    }
}
