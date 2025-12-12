namespace IAVH.BioTablero.CM.Core.Domain.Entities.Resources;

using System;

using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Resource Link entity.
/// </summary>
public class ResourceLink : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Resource identifier.
    /// </summary>
    public int ResourceId { get; set; }

    /// <summary>
    /// Entity name.
    /// </summary>
    public int Name { get; set; }

    /// <summary>
    /// Entity URL.
    /// </summary>
    public Uri Url { get; set; }

    /// <summary>
    /// Resource relationship.
    /// </summary>
    public Resource Resource { get; set; }
}
