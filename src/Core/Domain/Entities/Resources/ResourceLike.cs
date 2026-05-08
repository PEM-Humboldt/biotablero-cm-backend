namespace IAVH.BioTablero.CM.Core.Domain.Entities.Resources;

using System;

using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Resource Like entity.
/// </summary>
public class ResourceLike : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Resource identifier.
    /// </summary>
    public int ResourceId { get; set; }

    /// <summary>
    /// User Name identifier.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Entity creation date.
    /// </summary>
    public DateTimeOffset CreationDate { get; set; }

    /// <summary>
    /// Resource relationship.
    /// </summary>
    public Resource Resource { get; set; }
}
