namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Logging;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;
using IAVH.BioTablero.CM.WebApi.Interfaces;

using Microsoft.AspNetCore.Mvc;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Log Type controller.
/// </summary>
/// <param name="webTools">General web tools.</param>
/// <param name="entityService">Entity service.</param>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[ApiConventionType(typeof(CustomApiConventions))]
public class LogTypeController(IWebTools webTools,
    IReadEnumeration<LogType> entityService) : ControllerBase
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
