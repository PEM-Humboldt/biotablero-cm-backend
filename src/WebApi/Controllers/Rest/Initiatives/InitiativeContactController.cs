namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Initiatives;

using System;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.General;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Initiative contact controller
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class InitiativeContactController() : ControllerBase
{
    /// <summary>
    /// Get entity
    /// </summary>
    /// <param name="id">Entity identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Selected entity data</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(InitiativeContactDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
    public Task<IActionResult> Get(int id, CancellationToken ct) => throw new NotImplementedException();

    /// <summary>
    /// Add entity
    /// </summary>
    /// <param name="requestData">Request data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Added entity data</returns>
    [HttpPut]
    [Consumes("application/json")]
    public Task<IActionResult> Put([FromBody] object requestData, CancellationToken ct) => throw new NotImplementedException();

    /// <summary>
    /// Edit entity
    /// </summary>
    /// <param name="id">Entity identifier</param>
    /// <param name="entityData">Entity data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Updated entity data</returns>
    [HttpPost("{id}")]
    [Consumes("application/json")]
    public Task<IActionResult> Post(int id, [FromBody] object entityData, CancellationToken ct) => throw new NotImplementedException();

    /// <summary>
    /// Disable entity
    /// </summary>
    /// <param name="id">Entity identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    [HttpDelete("{id}")]
    public Task<IActionResult> Disable(int id, CancellationToken ct) => throw new NotImplementedException();
}
