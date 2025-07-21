namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.WebApi.Config.SwaggerSetup.Examples.LogsNS;
using IAVH.BioTablero.CM.WebApi.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Filters;

using static IAVH.BioTablero.CM.Core.Enums.LogEnums;

/// <summary>
/// Log type controller
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class LogTypeController(IWebTools webTools,
    IServiceReadEnumeration<LogType> entityService) : ControllerBase
{
    /// <summary>
    /// Get all entities
    /// </summary>
    /// <returns>Entities list from parameters</returns>
    [HttpGet]
    [ProducesResponseType(typeof(LogTypeResponseExample), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LogTypeResponseExample))]
    public IActionResult Get()
    {
        var response = entityService.GetAll();
        return webTools.CustomResponse(response);
    }
}
