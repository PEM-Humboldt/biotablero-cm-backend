namespace IAVH.BioTablero.CM.WebApi.Controllers.Mvc;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Email Templates controller.
/// </summary>
[Route("Email")]
[ApiExplorerSettings(IgnoreApi = true)]
public class EmailTemplatesController : Controller
{
    /// <summary>
    /// Get default view.
    /// </summary>
    /// <returns>Default view.</returns>
    [HttpGet]
    [Route("Default")]
    public IActionResult Default() => View();
}
