namespace IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Territory Story Video dto.
/// </summary>
public class TerritoryStoryVideoDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Territory Story identifier.
    /// </summary>
    public int? TerritoryStoryId { get; set; }

    /// <summary>
    /// File URL.
    /// </summary>
    public string FileUrl { get; set; }
}
