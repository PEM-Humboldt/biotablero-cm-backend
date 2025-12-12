namespace IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using IAVH.BioTablero.CM.Application.DTOs.Utils;
using IAVH.BioTablero.CM.Application.Interfaces.General;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.TagEnums;

/// <summary>
/// Tag dto.
/// </summary>
public class TagDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Tag name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Tag URL.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Tag Category relationship.
    /// </summary>
    public EnumEntityDto<TagCategory> Category { get; set; }
}
