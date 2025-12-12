namespace IAVH.BioTablero.CM.Core.Domain.Entities.Resources;

using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Resource Tag entity.
/// </summary>
public class ResourceTag : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Resource identifier.
    /// </summary>
    public int ResourceId { get; set; }

    /// <summary>
    /// Tag identifier.
    /// </summary>
    public int TagId { get; set; }

    /// <summary>
    /// Resource relationship.
    /// </summary>
    public Resource Resource { get; set; }

    /// <summary>
    /// Tag relationship.
    /// </summary>
    public Tag Tag { get; set; }
}
