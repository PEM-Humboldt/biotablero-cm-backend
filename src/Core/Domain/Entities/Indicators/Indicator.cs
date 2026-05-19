namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Indicator entity.
/// </summary>
public class Indicator : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public int InitiativeId { get; set; }

    /// <summary>
    /// Indicator Type identifier.
    /// </summary>
    public int IndicatorTypeId { get; set; }

    /// <summary>
    /// Initiative relationship.
    /// </summary>
    public Initiative Initiative { get; set; }

    /// <summary>
    /// Indicator Type relationship.
    /// </summary>
    public IndicatorType Type { get; set; }
}
