namespace IAVH.BioTablero.CM.WebApi.Utils.Requests;

using Microsoft.AspNetCore.Http;

/// <summary>
/// Territory Story Image request.
/// </summary>
public class TerritoryStoryImageRequest
{
    /// <summary>
    /// Territory Story identifier.
    /// </summary>
    public int TerritoryStoryId { get; set; }

    /// <summary>
    /// File description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// General file.
    /// </summary>
    public IFormFile File { get; set; }
}
