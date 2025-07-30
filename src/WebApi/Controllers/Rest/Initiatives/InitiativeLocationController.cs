namespace IAVH.BioTablero.CM.WebApi.Controllers.Rest.Initiatives;

using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

/// <summary>
/// Initiative-Location controller
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class InitiativeLocationController() : ODataController
{
    /// <summary>
    /// Add entity
    /// </summary>
    /// <param name="requestData">Request data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Added entity data</returns>
    [HttpPut]
    public Task<IActionResult> Put([FromBody] object requestData, CancellationToken ct) => throw new NotImplementedException();

    /// <summary>
    /// Delete entity
    /// </summary>
    /// <param name="id">Entity identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    [HttpDelete("{id}")]
    public Task<IActionResult> Delete(int id, CancellationToken ct) => throw new NotImplementedException();
}
