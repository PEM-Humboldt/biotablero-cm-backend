namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Users;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.Services.Users;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;
using IAVH.BioTablero.CM.WebApi.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// User controller.
/// </summary>
/// <param name="webTools">General web tools.</param>
/// <param name="userService">Inititive service.</param>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[ApiConventionType(typeof(CustomApiConventions))]
public class UserController(IWebTools webTools,
    IUserService userService) : ControllerBase
{
    /// <summary>
    /// Get all enabled users.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Selected entity data.</returns>
    [Authorize(Roles = IamConstants.RoleModuleAdmin)]
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var response = await userService.GetAllAsync(ct);
        return webTools.CustomResponse(response);
    }
}
