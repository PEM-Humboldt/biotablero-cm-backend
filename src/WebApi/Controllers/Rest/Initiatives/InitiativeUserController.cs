namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Initiatives;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.InitiativeUser;
using IAVH.BioTablero.CM.WebApi.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Initiative-User controller.
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[ApiConventionType(typeof(CustomApiConventions))]
public class InitiativeUserController(
    IWebTools webTools,
    IInitiativeUserService entityService) : ControllerBase
{
    /// <summary>
    /// Get entity.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Selected entity data.</returns>
    [HttpGet("{id}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeUserResponseExample))]
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
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeUserListResponseExample))]
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
    [Authorize(Roles = IamConstants.RoleModuleAdmin)]
    [SwaggerRequestExample(typeof(InitiativeUserDto), typeof(InitiativeUserAddRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeUserResponseExample))]
    public async Task<IActionResult> Put([FromBody] InitiativeUserDto requestData, CancellationToken ct)
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
    [SwaggerRequestExample(typeof(InitiativeUserDto), typeof(InitiativeUserEditRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeUserResponseExample))]
    public async Task<IActionResult> Post(int id, [FromBody] InitiativeUserDto requestData, CancellationToken ct)
    {
        var response = await entityService.UpdateAsync(id, requestData, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Delete entity.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = IamConstants.RoleModuleAdmin)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var response = await entityService.DeleteAsync(id, ct);
        return webTools.CustomResponse(response);
    }
}
