namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Auth;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Users;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Auth;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.InitiativeUser;
using IAVH.BioTablero.CM.WebApi.Interfaces;
using IAVH.BioTablero.CM.WebApi.Utils;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Authentication data controller.
/// </summary>
/// <param name="webTools">General web tools.</param>
/// <param name="initiativeService">Initiative service.</param>
/// <param name="initiativeUserService">Initiative User service.</param>
/// <param name="userService">User service.</param>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[ApiConventionType(typeof(CustomApiConventions))]
public class AuthController(IWebTools webTools,
    IInitiativeService initiativeService,
    IInitiativeUserService initiativeUserService,
    IUserService userService) : ControllerBase
{
    /// <summary>
    /// Get profile data from authenticated user.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>User profile data.</returns>
    [Authorize]
    [HttpGet("MyProfile")]
    [ProducesResponseType(typeof(ProfileDataResponseExample), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ProfileDataResponseExample))]
    public async Task<IActionResult> MyProfile(CancellationToken ct)
    {
        var response = await userService.GetProfileDataAsync(HttpContext.GetUserName(), ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Get initiatives data from authenticated user.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Selected entity data.</returns>
    [Authorize]
    [HttpGet("InitiativesData")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeLoggedUserDataResponseExample))]
    public async Task<IActionResult> GetListInitiativesData(CancellationToken ct)
    {
        var response = await initiativeService.GetByUserNameAsync(HttpContext.GetUserName(), ct);
        return webTools.CustomResponse(response);
    }

    /// <summary>
    /// Edit my focus area.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="requestData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Updated entity data.</returns>
    [HttpPut("MyFocusArea/{initiativeId}")]
    [Consumes("application/json")]
    [Authorize]
    [SwaggerRequestExample(typeof(InitiativeUserDto), typeof(InitiativeUserEditFocusAreaRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeUserResponseExample))]
    public async Task<IActionResult> EditMyFocusArea(int initiativeId, [FromBody] InitiativeUserDto requestData, CancellationToken ct)
    {
        var response = await initiativeUserService.UpdateFocusAreaAsync(initiativeId, HttpContext.GetUserName(), requestData, ct);
        return webTools.CustomResponse(response);
    }
}
