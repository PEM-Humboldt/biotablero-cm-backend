namespace IAVH.BioTablero.CM.WebApi.Interfaces;

using System;

using IAVH.BioTablero.CM.Application.Utils;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Custom web tools.
/// </summary>
public interface IWebTools
{
    /// <summary>
    /// Get base URL.
    /// </summary>
    /// <returns>Current project base URL.</returns>
    Uri GetBaseUrl();

    /// <summary>
    /// Generate custom http response.
    /// </summary>
    /// <param name="response">Model with response data.</param>
    /// <returns>HTTP response with custom parameters.</returns>
    IActionResult CustomResponse(CustomWebResponse response);
}
