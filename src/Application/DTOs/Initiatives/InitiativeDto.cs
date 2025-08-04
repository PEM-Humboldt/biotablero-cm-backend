namespace IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Initiative entity
/// </summary>
public class InitiativeDto : IDto
{
    /// <summary>
    /// Item identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Initiative name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Initiative description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Initiative logo URL
    /// </summary>
    public Uri LogoUrl { get; set; }

    /// <summary>
    /// Initiative Locations relationship
    /// </summary>
    public ICollection<InitiativeLocationDto> InitiativeLocations { get; } = [];

    /// <summary>
    /// Initiative Contacts relationship
    /// </summary>
    public ICollection<InitiativeContactDto> InitiativeContacts { get; } = [];

    /// <summary>
    /// Initiative Users relationship
    /// </summary>
    public ICollection<InitiativeUserDto> InitiativeUsers { get; } = [];
}
