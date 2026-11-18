namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Initiatives;

using System;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.JoinInvitation;
using IAVH.BioTablero.CM.WebApi.Interfaces;
using IAVH.BioTablero.CM.WebApi.Utils;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

/// <summary>
/// Join Invitation controller.
/// </summary>
[Authorize]
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[ApiConventionType(typeof(CustomApiConventions))]
public class JoinInvitationController(
    IWebTools webTools,
    IJoinInvitationService entityService) : ControllerBase
{
    /// <summary>
    /// Get entities (paginated).
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities list from parameters.</returns>
    [HttpGet]
    [OpenApiResponse(StatusCodes.Status200OK, typeof(JoinInvitationOdataResponseExample))]
    public async Task<IActionResult> GetOdataList(int initiativeId, ODataQueryOptions<JoinInvitation> queryOptions, CancellationToken ct)
    {
        var response = await entityService.GetListAsync(initiativeId, HttpContext.GetUserName(), queryOptions, ct);
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
    [OpenApiRequest(typeof(JoinInvitationAddRequestExample))]
    [OpenApiResponse(StatusCodes.Status200OK, typeof(JoinInvitationResponseExample))]
    public async Task<IActionResult> Post([FromBody] JoinInvitationDto requestData, CancellationToken ct)
    {
        var userName = HttpContext.GetUserName();
        ArgumentException.ThrowIfNullOrEmpty(userName);

        requestData.Creator = userName;
        var response = await entityService.AddAsync(requestData, ct);
        return webTools.CustomResponse(response);
    }
}
