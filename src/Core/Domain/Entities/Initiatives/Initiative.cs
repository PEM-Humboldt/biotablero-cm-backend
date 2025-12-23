namespace IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

using NetTopologySuite.Geometries;

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
    /// Initiative short name.
    /// </summary>
    public string ShortName { get; set; }

    /// <summary>
    /// Initiative description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Initiative influence area.
    /// </summary>
    public string InfluenceArea { get; set; }

    /// <summary>
    /// Initiative objective.
    /// </summary>
    public string Objective { get; set; }

    /// <summary>
    /// Initiative creation date.
    /// </summary>
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// Initiative image URL.
    /// </summary>
    public Uri ImageUrl { get; set; }

    /// <summary>
    /// Initiative banner URL.
    /// </summary>
    public Uri BannerUrl { get; set; }

    /// <summary>
    /// Initiative polygon.
    /// </summary>
    public Geometry Polygon { get; set; }

    /// <summary>
    /// Initiative polygon centroid.
    /// </summary>
    public Point Coordinate { get; set; }

    /// <summary>
    /// Initiative polygon area in square kilometers.
    /// </summary>
    public double PolygonArea { get; set; }

    /// <summary>
    /// Enabled flag.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Initiative Location relationship.
    /// </summary>
    public ICollection<InitiativeLocation> InitiativeLocations { get; init; }

    /// <summary>
    /// Initiative Contact relationship.
    /// </summary>
    public ICollection<InitiativeContact> InitiativeContacts { get; init; }

    /// <summary>
    /// Initiative User relationship.
    /// </summary>
    public ICollection<InitiativeUser> InitiativeUsers { get; init; }

    /// <summary>
    /// Initiative Tag relationship.
    /// </summary>
    public ICollection<InitiativeTag> InitiativeTags { get; init; }

    /// <summary>
    /// Join Request relationship.
    /// </summary>
    public ICollection<JoinRequest> JoinRequests { get; init; }

    /// <summary>
    /// Join Invitation relationship.
    /// </summary>
    public ICollection<JoinInvitation> JoinInvitations { get; init; }

    /// <summary>
    /// Territory Story relationship.
    /// </summary>
    public ICollection<TerritoryStory> TerritoryStories { get; init; }

    /// <summary>
    /// Resource relationship.
    /// </summary>
    public ICollection<Resource> Resources { get; init; }
}
