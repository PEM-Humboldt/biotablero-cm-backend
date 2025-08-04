namespace IAVH.BioTablero.CM.Core.Domain.Initiatives;

using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Domain.Geo;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Initiative Location entity
/// </summary>
public class InitiativeLocation : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Initiative identifier
    /// </summary>
    public int InitiativeId { get; set; }

    /// <summary>
    /// Location identifier
    /// </summary>
    public int LocationId { get; set; }

    /// <summary>
    /// Locality
    /// </summary>
    public string Locality { get; set; }

    /// <summary>
    /// Initiative relationship
    /// </summary>
    public Initiative Initiative { get; set; }

    /// <summary>
    /// Location relationship
    /// </summary>
    public Location Location { get; set; }
}
