namespace IAVH.BioTablero.CM.WebApi.Utils.Requests.ResourceFile;

using Microsoft.AspNetCore.Http;

/// <summary>
/// Resource file edit request.
/// </summary>
public class ResourceFileEditRequest
{
    /// <summary>
    /// File name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// General file.
    /// </summary>
    public required IFormFile File { get; set; }
}
