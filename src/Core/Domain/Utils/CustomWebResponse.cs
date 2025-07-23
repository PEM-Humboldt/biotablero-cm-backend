namespace IAVH.BioTablero.CM.Core.Domain.Utils;

using System.Net;

/// <summary>
/// Custom HTTP response model
/// </summary>
/// <param name="error">Error boolean flag</param>
public class CustomWebResponse(bool error = false)
{
    /// <summary>
    /// Response success code
    /// </summary>
    public bool Success { get; } = !error;

    /// <summary>
    /// Response status code
    /// </summary>
    public HttpStatusCode StatusCode { get; init; } = error ? HttpStatusCode.BadRequest : HttpStatusCode.OK;

    /// <summary>
    /// Custom response body
    /// </summary>
    public object ResponseBody { get; init; }

    /// <summary>
    /// Custom response error message
    /// </summary>
    public string Message { get; init; }
}
