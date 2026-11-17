namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Initiatives;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;

using IAVH.BioTablero.CM.WebApi.Interfaces;

using Microsoft.AspNetCore.Mvc;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Initiative User Level controller.
/// </summary>
/// <param name="webTools">General web tools.</param>
/// <param name="entityService">Entity service.</param>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[ApiConventionType(typeof(CustomApiConventions))]
public class InitiativeUserLevelController(IWebTools webTools,
    IReadEnumeration<InitiativeUserLevel> entityService) : ControllerBase
{
    /// <summary>
    /// Get all entities.
    /// </summary>
    /// <returns>Entities list from parameters.</returns>
    [HttpGet]
    public IActionResult GetEnumList()
    {
        var response = entityService.GetAll();
        return webTools.CustomResponse(response);
    }
}
