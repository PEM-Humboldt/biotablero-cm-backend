namespace IAVH.BioTablero.CM.Core.Domain.Entities.Geo;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Location entity.
/// </summary>
public class Location : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Location name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Location code.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Parent location identifier.
    /// </summary>
    public int? ParentId { get; set; }

    /// <summary>
    /// Location level.
    /// </summary>
    public byte Level { get; set; }

    /// <summary>
    /// Parent location relationship.
    /// </summary>
    public Location Parent { get; set; }

    /// <summary>
    /// Location polygon relationship.
    /// </summary>
    public LocationPolygon LocationPolygon { get; set; }

    /// <summary>
    /// Child locations relationship.
    /// </summary>
    public ICollection<Location> Children { get; } = [];

    /// <summary>
    /// Initiative Users relationship.
    /// </summary>
    public ICollection<InitiativeLocation> InitiativeLocations { get; } = [];
}
