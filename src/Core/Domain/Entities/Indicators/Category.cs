namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Category entity.
/// </summary>
public class Category : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Parent identifier.
    /// </summary>
    public int? ParentId { get; set; }

    /// <summary>
    /// Category name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Category description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Parent relationship.
    /// </summary>
    public Category Parent { get; set; }

    /// <summary>
    /// Child categories relationship.
    /// </summary>
    public ICollection<Category> Children { get; } = [];

    /// <summary>
    /// Indicator Group relationship.
    /// </summary>
    public ICollection<IndicatorGroup> IndicatorGroups { get; init; }
}
