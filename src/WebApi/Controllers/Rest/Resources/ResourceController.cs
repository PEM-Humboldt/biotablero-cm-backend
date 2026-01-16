namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Resources;

using System.ComponentModel.Design;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples;
using IAVH.BioTablero.CM.WebApi.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

/// <summary>
/// Resource controller.
/// </summary>
/// <param name="webTools">General web tools.</param>
/// <param name="entityService">Entity service.</param>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[ApiConventionType(typeof(CustomApiConventions))]
public class ResourceController(
    IWebTools webTools,
    IResourceService entityService) : ODataController
{
}
