namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.TerritoryStories;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Application.Interfaces.Services.TerritoryStory;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Storage;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.TerritoryStoryImage;
using IAVH.BioTablero.CM.WebApi.Interfaces;
using IAVH.BioTablero.CM.WebApi.Utils;
using IAVH.BioTablero.CM.WebApi.Utils.Requests.TerritoryStoryImage;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Territory Story Image controller.
/// </summary>
/// <param name="webTools">General web tools.</param>
/// <param name="entityService">Entity service.</param>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[ApiConventionType(typeof(CustomApiConventions))]
public class TerritoryStoryImageController(
    IWebTools webTools,
    ITerritoryStoryImageService entityService) : ODataController
{
    /// <summary>
    /// Get entity.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Selected entity data.</returns>
    [HttpGet("{id}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TerritoryStoryImageResponseExample))]
    public async Task<IActionResult> GetItem(int id, CancellationToken ct)
    {
        var response = await entityService.GetItemAsync(id, HttpContext.GetUserName(), ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Get entities by Territory Story.
    /// </summary>
    /// <param name="territoryStoryId">Territory Story identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities list from parameters.</returns>
    [HttpGet("GetByTerritoryStory/{territoryStoryId}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TerritoryStoryImageListResponseExample))]
    public async Task<IActionResult> GetListByTerritoryStory(int territoryStoryId, CancellationToken ct)
    {
        var response = await entityService.GetByTerritoryStoryAsync(territoryStoryId, ct);
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
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TerritoryStoryImageResponseExample))]
    public async Task<IActionResult> Put([FromForm] TerritoryStoryImageAddRequest requestData, CancellationToken ct)
    {
        var requestDataDto = new TerritoryStoryImageDto()
        {
            TerritoryStoryId = requestData.TerritoryStoryId,
            Description = requestData.Description,
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
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TerritoryStoryImageResponseExample))]
    public async Task<IActionResult> Post(int id, [FromForm] TerritoryStoryImageEditRequest requestData, CancellationToken ct)
    {
        var requestDataDto = new TerritoryStoryImageDto()
        {
            Description = requestData.Description,
        };

        var response = await entityService.UpdateAsync(id, HttpContext.GetUserName(), requestDataDto, new FormFileAdapter(requestData.File), ct);
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
