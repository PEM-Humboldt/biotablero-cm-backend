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
    public string Name { get; set; }

    /// <summary>
    /// General file.
    /// </summary>
    public IFormFile File { get; set; }
}
