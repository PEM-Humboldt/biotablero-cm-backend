namespace IAVH.BioTablero.CM.Application.DTOs.Resources;

using System;
using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Resource File dto.
/// </summary>
[method: SetsRequiredMembers]
public class ResourceFileDto() : IDto
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
    public required string Name { get; set; } = string.Empty;

    /// <summary>
    /// Entity URL.
    /// </summary>
    public required Uri Url { get; set; } = new(string.Empty);
}
