namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Initiatives;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;
using IAVH.BioTablero.CM.WebApi.Interfaces;
using IAVH.BioTablero.CM.WebApi.Utils;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

using Swashbuckle.AspNetCore.Filters;

using InitiativeJoinRequestStatusEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeJoinRequestStatus;

/// <summary>
/// Initiative Join Request controller.
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class InitiativeJoinRequestController(
    IWebTools webTools,
    IInitiativeJoinRequestService entityService) : ControllerBase
{
    /// <summary>
    /// Get entities (paginated).
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities list from parameters.</returns>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(InitiativeJoinRequestOdataResponseExample), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeJoinRequestOdataResponseExample))]
    public async Task<IActionResult> Get(int initiativeId, ODataQueryOptions<InitiativeJoinRequest> queryOptions, CancellationToken ct)
    {
        var response = await entityService.GetList(initiativeId, HttpContext.GetUserName(), queryOptions, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Add entity.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Added entity data.</returns>
    [HttpPut]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(InitiativeJoinRequestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeJoinRequestDto))]
    public async Task<IActionResult> Put(int initiativeId, CancellationToken ct)
    {
        var requestData = new InitiativeJoinRequestDto()
        {
            InitiativeId = initiativeId,
            UserName = HttpContext.GetUserName(),
        };

        var response = await entityService.Add(requestData, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Edit entity.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="requestStatus">Join request status.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Updated entity data.</returns>
    [HttpPost("{id}")]
    [Consumes("application/json")]
    [Authorize]
    [ProducesResponseType(typeof(InitiativeJoinRequestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeJoinRequestDto))]
    public async Task<IActionResult> Post(int id, InitiativeJoinRequestStatusEnum requestStatus, CancellationToken ct)
    {
        var requestData = new InitiativeJoinRequestDto()
        {
            Status = new EnumEntityDto<InitiativeJoinRequestStatusEnum>(requestStatus),
            ReviewerUserName = HttpContext.GetUserName(),
        };

        var response = await entityService.Update(id, requestData, ct);
        return webTools.CustomResponse(response);
    }
}
