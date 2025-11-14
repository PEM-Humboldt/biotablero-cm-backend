namespace IAVH.BioTablero.CM.WebApi.Utils.Requests;

/// <summary>
/// Territory Story Image request.
/// </summary>
public class TerritoryStoryImageRequest : UploadFileRequest
{
    /// <summary>
    /// Territory Story identifier.
    /// </summary>
    public int TerritoryStoryId { get; set; }
}
