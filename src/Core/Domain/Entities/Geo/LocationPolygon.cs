namespace IAVH.BioTablero.CM.Core.Domain.Entities.Geo;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

using NetTopologySuite.Geometries;

/// <summary>
/// LocationPolygon entity.
/// </summary>
public class LocationPolygon : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Geometry.
    /// </summary>
    public Geometry Geometry { get; set; }

    /// <summary>
    /// Simplified geometry.
    /// </summary>
    public string GeometrySimplified { get; set; }

    /// <summary>
    /// Location identifier.
    /// </summary>
    public int LocationId { get; set; }

    /// <summary>
    /// Location relationship.
    /// </summary>
    public Location Location { get; set; }
}
