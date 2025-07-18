namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.WebApi.Interfaces;

using Microsoft.AspNetCore.Mvc;

using static IAVH.BioTablero.CM.Core.Enums.LogEnums;

/// <summary>
/// General bot controller
/// </summary>
[ApiController]
[Route("[controller]")]
public class LogTypeController(IWebTools webTools,
    IServiceReadEnum<LogType> entityService) : ControllerBase
{
    /// <summary>
    /// Get all entities
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Entities list from parameters</returns>
    [HttpGet]
    public IActionResult Get()
    {
        var response = entityService.GetAll();
        return webTools.CustomResponse(response);
    }
}
