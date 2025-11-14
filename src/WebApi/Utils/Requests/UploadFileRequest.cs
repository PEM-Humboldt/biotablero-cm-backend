namespace IAVH.BioTablero.CM.WebApi.Utils.Requests;

using Microsoft.AspNetCore.Http;

/// <summary>
/// General upload file request.
/// </summary>
public class UploadFileRequest
{
    /// <summary>
    /// General entity identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// File name (optional).
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// File description (optional).
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// General file.
    /// </summary>
    public IFormFile File { get; set; }
}
