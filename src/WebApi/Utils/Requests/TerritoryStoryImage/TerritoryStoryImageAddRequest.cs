namespace IAVH.BioTablero.CM.WebApi.Utils.Requests.TerritoryStoryImage;

/// <summary>
/// Territory Story Image add request.
/// </summary>
public class TerritoryStoryImageAddRequest : TerritoryStoryImageEditRequest
{
    /// <summary>
    /// Territory Story identifier.
    /// </summary>
    public int TerritoryStoryId { get; set; }
}
