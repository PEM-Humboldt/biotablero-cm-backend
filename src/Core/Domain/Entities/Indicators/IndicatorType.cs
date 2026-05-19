namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Indicator Type entity.
/// </summary>
public class IndicatorType : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Entity name.
    /// </summary>
    public string Name { get; set; }
}
