namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Initiatives;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Initiatives;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.InitiativeContact;
using IAVH.BioTablero.CM.WebApi.Interfaces;
using IAVH.BioTablero.CM.WebApi.Utils;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Initiative Contact controller.
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[ApiConventionType(typeof(CustomApiConventions))]
public class InitiativeContactController(
    IWebTools webTools,
    IInitiativeContactService entityService) : ControllerBase
{
    /// <summary>
    /// Get entity.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Selected entity data.</returns>
    [HttpGet("{id}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeContactResponseExample))]
    public async Task<IActionResult> GetItem(int id, CancellationToken ct)
    {
        var response = await entityService.GetItemAsync(id, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Get entities by Initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities list from parameters.</returns>
    [HttpGet("GetByInitiative/{initiativeId}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeContactListResponseExample))]
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
    [HttpPost]
    [Consumes("application/json")]
    [Authorize]
    [SwaggerRequestExample(typeof(InitiativeContactDto), typeof(InitiativeContactAddRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeContactResponseExample))]
    public async Task<IActionResult> Post([FromBody] InitiativeContactDto requestData, CancellationToken ct)
    {
        var response = await entityService.AddAsync(HttpContext.GetUserName(), HttpContext.UserIsAdmin(), requestData, ct);
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
    [SwaggerRequestExample(typeof(InitiativeContactDto), typeof(InitiativeContactEditRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeContactResponseExample))]
    public async Task<IActionResult> Put(int id, [FromBody] InitiativeContactDto requestData, CancellationToken ct)
    {
        var response = await entityService.UpdateAsync(id, HttpContext.GetUserName(), HttpContext.UserIsAdmin(), requestData, ct);
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
        var response = await entityService.DeleteAsync(id, HttpContext.GetUserName(), HttpContext.UserIsAdmin(), ct);
        return webTools.CustomResponse(response);
    }
}
