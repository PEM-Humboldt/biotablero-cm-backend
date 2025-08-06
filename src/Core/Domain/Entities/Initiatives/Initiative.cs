namespace IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Initiative entity.
/// </summary>
public class Initiative : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Initiative name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Initiative description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Initiative logo URL.
    /// </summary>
    public Uri LogoUrl { get; set; }

    /// <summary>
    /// Initiative Locations relationship.
    /// </summary>
    public ICollection<InitiativeLocation> InitiativeLocations { get; init; } = [];

    /// <summary>
    /// Initiative Contacts relationship.
    /// </summary>
    public ICollection<InitiativeContact> InitiativeContacts { get; init; } = [];

    /// <summary>
    /// Initiative Users relationship.
    /// </summary>
    public ICollection<InitiativeUser> InitiativeUsers { get; init; } = [];
}
