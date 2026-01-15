namespace IAVH.BioTablero.CM.Application.DTOs.Resources;

using System;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Resource Link dto.
/// </summary>
public class ResourceLinkDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Resource identifier.
    /// </summary>
    public int ResourceId { get; set; }

    /// <summary>
    /// Entity name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Entity URL.
    /// </summary>
    public Uri Url { get; set; }
}
