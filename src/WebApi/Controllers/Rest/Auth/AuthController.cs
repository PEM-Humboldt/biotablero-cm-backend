namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Auth;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;
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
/// <param name="initiativeUserService">Entity service.</param>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[ApiConventionType(typeof(CustomApiConventions))]
public class AuthController(IWebTools webTools,
    IInitiativeUserService initiativeUserService) : ControllerBase
{
    /// <summary>
    /// Get inititives data from authenticated user.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Selected entity data.</returns>
    [Authorize]
    [HttpGet("InitiativesData")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InitiativeUserListResponseExample))]
    public async Task<IActionResult> GetInitiativesData(CancellationToken ct)
    {
        var response = await initiativeUserService.GetByUserNameAsync(HttpContext.GetUserName(), ct);
        return webTools.CustomResponse(response);
    }
}
