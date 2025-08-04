namespace IAVH.BioTablero.CM.Core.Domain.Initiatives;

using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Initiative Contact entity
/// </summary>
public class InitiativeContact : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Initiative identifier
    /// </summary>
    public int InitiativeId { get; set; }

    /// <summary>
    /// Phone number
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// Email address
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Initiative relationship
    /// </summary>
    public Initiative Initiative { get; set; }
}
