namespace IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

using System;

using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Territory Story Like entity.
/// </summary>
public class TerritoryStoryLike : BaseEntity<int>, IAggregateRoot
{
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
    public DateTimeOffset CreationDate { get; set; }

    /// <summary>
    /// Territory Story relationship.
    /// </summary>
    public TerritoryStory TerritoryStory { get; set; }
}
