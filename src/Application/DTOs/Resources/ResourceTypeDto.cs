namespace IAVH.BioTablero.CM.Application.DTOs.Resources;

using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Resource Type dto.
/// </summary>
[method: SetsRequiredMembers]
public class ResourceTypeDto() : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Entity name.
    /// </summary>
    public required string Name { get; set; } = string.Empty;

    /// <summary>
    /// Entity description.
    /// </summary>
    public string? Description { get; set; }
}
