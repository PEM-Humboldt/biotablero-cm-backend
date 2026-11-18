namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Initiatives;

using System;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.JoinRequest;
using IAVH.BioTablero.CM.WebApi.Interfaces;
using IAVH.BioTablero.CM.WebApi.Utils;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

using JoinRequestStatusEnum = Core.Domain.Utils.Enums.InitiativesEnums.JoinRequestStatus;

/// <summary>
/// Join Request controller.
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[ApiConventionType(typeof(CustomApiConventions))]
public class JoinRequestController(
    IWebTools webTools,
    IJoinRequestService entityService) : ControllerBase
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
    [OpenApiResponse(StatusCodes.Status200OK, typeof(JoinRequestOdataResponseExample))]
    public async Task<IActionResult> GetOdataList(int initiativeId, ODataQueryOptions<JoinRequest> queryOptions, CancellationToken ct)
    {
        var response = await entityService.GetListAsync(initiativeId, HttpContext.GetUserName(), queryOptions, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Get my join requests.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Join Requests list.</returns>
    [Authorize]
    [HttpGet("MyRequests")]
    [OpenApiResponse(StatusCodes.Status200OK, typeof(JoinRequestListResponseExample))]
    public async Task<IActionResult> GetListInitiativesData(CancellationToken ct)
    {
        var response = await entityService.GetByUserNameAsync(HttpContext.GetUserName(), ct);
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
    [OpenApiRequest(typeof(JoinRequestAddRequestExample))]
    [OpenApiResponse(StatusCodes.Status200OK, typeof(JoinRequestResponseExample))]
    public async Task<IActionResult> Post([FromBody] JoinRequestDto requestData, CancellationToken ct)
    {
        var userName = HttpContext.GetUserName();
        ArgumentException.ThrowIfNullOrEmpty(userName);

        requestData.UserName = userName;
        var response = await entityService.AddAsync(requestData, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Edit entity.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="requestStatus">Join request status.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Updated entity data.</returns>
    [HttpPut("{id}")]
    [Authorize]
    [OpenApiResponse(StatusCodes.Status200OK, typeof(JoinRequestResponseExample))]
    public async Task<IActionResult> Put(int id, JoinRequestStatusEnum requestStatus, CancellationToken ct)
    {
        var userName = HttpContext.GetUserName();
        ArgumentException.ThrowIfNullOrEmpty(userName);

        var requestData = new JoinRequestDto()
        {
            Status = new EnumEntityDto<JoinRequestStatusEnum>(requestStatus),
            ReviewerUserName = userName,
        };

        var response = await entityService.UpdateAsync(id, requestData, ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Cancel request.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    [HttpDelete("Cancel/{id}")]
    [Authorize]
    public async Task<IActionResult> Cancel(int id, CancellationToken ct)
    {
        var response = await entityService.CancelAsync(id, HttpContext.GetUserName(), ct);
        return webTools.CustomResponse(response);
    }
}
