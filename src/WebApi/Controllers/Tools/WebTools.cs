namespace IAVH.BioTablero.CM.WebApi.Controllers.Tools;

using System;
using System.Collections.Generic;
using System.Net;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
using IAVH.BioTablero.CM.WebApi.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Custom web tools.
/// </summary>
public sealed class WebTools(IHttpContextAccessor httpContextAccessor) : ControllerBase, IWebTools
{
    /// <inheritdoc/>
    [ApiExplorerSettings(IgnoreApi = true)]
    public Uri GetBaseUrl()
    {
        var context = httpContextAccessor.HttpContext;
        return new Uri($"{context?.Request.Scheme}://{context?.Request.Host.Value}{context?.Request.PathBase.Value}/");
    }

    /// <inheritdoc/>
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult CustomResponse(CustomWebResponse response)
    {
        if (response?.Success ?? false)
        {
            return Ok(response.ResponseBody);
        }

        // Add custom error message for validations
        if (string.IsNullOrEmpty(response.Message) && response.StatusCode == HttpStatusCode.BadRequest && response.ResponseBody is IEnumerable<ApiValidationError>)
        {
            response.Message = ValidationErrorCodes.ValidationErrorsMsg;
        }

        var errorObject = new
        {
            error = response.Message,
            data = response.ResponseBody,
        };

        return StatusCode((int)response.StatusCode, errorObject);
    }
}
