namespace IAVH.BioTablero.CM.WebApi.Utils.Requests.ResourceFile;

/// <summary>
/// Resource file add request.
/// </summary>
public class ResourceFileAddRequest : ResourceFileEditRequest
{
    /// <summary>
    /// Resource identifier.
    /// </summary>
    public int ResourceId { get; set; }
}
