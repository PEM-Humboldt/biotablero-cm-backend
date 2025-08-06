namespace IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Initiative entity.
/// </summary>
public class InitiativeDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

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
    public IEnumerable<InitiativeLocationDto> InitiativeLocations { get; init; } = [];

    /// <summary>
    /// Initiative Contacts relationship.
    /// </summary>
    public IEnumerable<InitiativeContactDto> InitiativeContacts { get; init; } = [];

    /// <summary>
    /// Initiative Users relationship.
    /// </summary>
    public IEnumerable<InitiativeUserDto> InitiativeUsers { get; init; } = [];
}
