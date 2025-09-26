namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Auth;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Auth;
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
/// <param name="initiativeService">Inititive service.</param>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[ApiConventionType(typeof(CustomApiConventions))]
public class AuthController(IWebTools webTools,
    IInitiativeService initiativeService) : ControllerBase
{
    /// <summary>
    /// Get inititives data from authenticated user.
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
}
