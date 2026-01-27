namespace IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Initiative dto.
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
    /// Initiative short name.
    /// </summary>
    public string ShortName { get; set; }

    /// <summary>
    /// Initiative description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Initiative baseline.
    /// </summary>
    public string Baseline { get; set; }

    /// <summary>
    /// Initiative objective.
    /// </summary>
    public string Objective { get; set; }

    /// <summary>
    /// Initiative creation date.
    /// </summary>
    public DateTime? CreationDate { get; set; }

    /// <summary>
    /// Initiative image URL.
    /// </summary>
    public Uri ImageUrl { get; set; }

    /// <summary>
    /// Initiative banner URL.
    /// </summary>
    public Uri BannerUrl { get; set; }

    /// <summary>
    /// Initiative polygon centroid.
    /// </summary>
    public double[] Coordinate { get; set; }

    /// <summary>
    /// Initiative polygon area in square kilometers.
    /// </summary>
    public double? PolygonArea { get; set; }

    /// <summary>
    /// Enabled flag.
    /// </summary>
    public bool? Enabled { get; set; }

    /// <summary>
    /// Initiative Locations relationship.
    /// </summary>
    public IEnumerable<InitiativeLocationDto> Locations { get; init; }

    /// <summary>
    /// Initiative Contacts relationship.
    /// </summary>
    public IEnumerable<InitiativeContactDto> Contacts { get; init; }

    /// <summary>
    /// Initiative Users relationship.
    /// </summary>
    public IEnumerable<InitiativeUserDto> Users { get; init; }

    /// <summary>
    /// Tags relationship.
    /// </summary>
    public IEnumerable<InitiativeTagDto> Tags { get; init; }
}
