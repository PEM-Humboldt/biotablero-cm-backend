namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Initiatives;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.General;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;
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
    [ProducesResponseType(typeof(InitiativeUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
    public async Task<IActionResult> Get(int id, CancellationToken ct)
    {
        var response = await entityService.GetItem(id, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Get entities by Initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities list from parameters.</returns>
    [HttpGet("GetByInitiative/{initiativeId}")]
    [ProducesResponseType(typeof(IEnumerable<InitiativeUserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByInitiative(int initiativeId, CancellationToken ct)
    {
        var response = await entityService.GetByInitiative(initiativeId, ct);
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
    [ProducesResponseType(typeof(InitiativeUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeUserDto))]
    public async Task<IActionResult> Put([FromBody] InitiativeUserDto requestData, CancellationToken ct)
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
    [SwaggerRequestExample(typeof(InitiativeUserDto), typeof(InitiativeUserEditRequestExample))]
    [ProducesResponseType(typeof(InitiativeUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeUserDto))]
    public async Task<IActionResult> Post(int id, [FromBody] InitiativeUserDto requestData, CancellationToken ct)
    {
        var response = await entityService.Update(id, requestData, ct);
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
    [SwaggerRequestExample(typeof(InitiativeUserDto), typeof(InitiativeUserEditRequestExample))]
    [ProducesResponseType(typeof(InitiativeUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeUserDto))]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var response = await entityService.Delete(id, ct);
        return webTools.CustomResponse(response);
    }
}
