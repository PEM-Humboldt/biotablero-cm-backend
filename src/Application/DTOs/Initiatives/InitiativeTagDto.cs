namespace IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Utils;
using IAVH.BioTablero.CM.Application.Interfaces.General;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Initiative Tag dto.
/// </summary>
public class InitiativeTagDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Initiative Tag name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Initiative Tag URL.
    /// </summary>
    public Uri Url { get; set; }

    /// <summary>
    /// Category relationship.
    /// </summary>
    public EnumEntityDto<InitiativeTagCategory> Category { get; set; }
}
