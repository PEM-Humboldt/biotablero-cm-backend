namespace IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;

using System;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Territory Story Like dto.
/// </summary>
public class TerritoryStoryLikeDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Territory Story identifier.
    /// </summary>
    public int TerritoryStoryId { get; set; }

    /// <summary>
    /// User Name identifier.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Entity creation date.
    /// </summary>
    public DateTimeOffset? CreationDate { get; set; }
}
