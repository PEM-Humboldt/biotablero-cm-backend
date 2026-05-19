namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using System;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Indicator Version entity.
/// </summary>
public class IndicatorVersion : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Indicator identifier.
    /// </summary>
    public int IndicatorId { get; set; }

    /// <summary>
    /// Creation date.
    /// </summary>
    public DateTimeOffset CreationDate { get; set; }

    /// <summary>
    /// Version number.
    /// </summary>
    public int Version { get; set; }

    /// <summary>
    /// Indicator description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Indicator methodology.
    /// </summary>
    public string Methodology { get; set; }

    /// <summary>
    /// Indicator interpretation.
    /// </summary>
    public string Interpretation { get; set; }

    /// <summary>
    /// Indicator considerations.
    /// </summary>
    public string Considerations { get; set; }

    /// <summary>
    /// Indicator autorship.
    /// </summary>
    public string Authorship { get; set; }

    /// <summary>
    /// Indicator relationship.
    /// </summary>
    public Indicator Indicator { get; set; }
}
