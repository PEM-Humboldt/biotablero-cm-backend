namespace IAVH.BioTablero.CM.Core.Domain.Initiatives;

using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Initiative User entity
/// </summary>
public class InitiativeUser : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Initiative identifier
    /// </summary>
    public int InitiativeId { get; set; }

    /// <summary>
    /// User identifier
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Level identifier
    /// </summary>
    public int LevelId { get; set; }

    /// <summary>
    /// Initiative relationship
    /// </summary>
    public Initiative Initiative { get; set; }

    /// <summary>
    /// Level relationship
    /// </summary>
    public InitiativeUserLevel Level { get; set; }
}
