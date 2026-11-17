namespace IAVH.BioTablero.CM.Application.DTOs.Tags;

using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.DTOs.Utils;
using IAVH.BioTablero.CM.Application.Interfaces.General;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.TagEnums;

/// <summary>
/// Tag dto.
/// </summary>
[method: SetsRequiredMembers]
public class TagDto() : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Tag name.
    /// </summary>
    public required string Name { get; set; } = string.Empty;

    /// <summary>
    /// Tag full name.
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// Tag URL.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Tag Category relationship.
    /// </summary>
    public required EnumEntityDto<TagCategory> Category { get; set; } = new();
}
