namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest;

using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

/// <summary>
/// Initiative contact controller
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class InitiativeContactController() : ODataController
{
    /// <summary>
    /// Get entity
    /// </summary>
    /// <param name="id">Entity identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Selected entity data</returns>
    [HttpGet("{id}")]
    public Task<IActionResult> Get(int id, CancellationToken ct) => throw new NotImplementedException();

    /// <summary>
    /// Get entities (paginated)
    /// </summary>
    /// <param name="queryOptions">OData query options</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Entities list from parameters</returns>
    [HttpGet]
    public Task<IActionResult> Get(ODataQueryOptions<object> queryOptions, CancellationToken ct) => throw new NotImplementedException();

    /// <summary>
    /// Add entity
    /// </summary>
    /// <param name="requestData">Request data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Added entity data</returns>
    [HttpPut]
    public Task<IActionResult> Put([FromBody] object requestData, CancellationToken ct) => throw new NotImplementedException();

    /// <summary>
    /// Edit entity
    /// </summary>
    /// <param name="id">Entity identifier</param>
    /// <param name="entityData">Entity data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Updated entity data</returns>
    [HttpPost("{id}")]
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
