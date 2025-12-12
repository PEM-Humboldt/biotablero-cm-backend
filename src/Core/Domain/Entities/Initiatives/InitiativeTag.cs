namespace IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Initiative Tag entity.
/// </summary>
public class InitiativeTag : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public int InitiativeId { get; set; }

    /// <summary>
    /// Initiative Tag identifier.
    /// </summary>
    public int TagId { get; set; }

    /// <summary>
    /// Initiative relationship.
    /// </summary>
    public Initiative Initiative { get; set; }

    /// <summary>
    /// Initiative Tag relationship.
    /// </summary>
    public Tag Tag { get; set; }
}
