namespace IAVH.BioTablero.CM.WebApi.Utils.Requests.TerritoryStoryImage;

using Microsoft.AspNetCore.Http;

/// <summary>
/// Territory Story Image edit request.
/// </summary>
public class TerritoryStoryImageEditRequest
{
    /// <summary>
    /// File description.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// General file.
    /// </summary>
    public required IFormFile File { get; set; }
}
