namespace IAVH.BioTablero.CM.Core.Domain.Entities.Resources;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Resource Type entity.
/// </summary>
public class ResourceType : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Entity name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Entity description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Resource relationship.
    /// </summary>
    public ICollection<Resource> Resources { get; init; }
}
