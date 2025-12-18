namespace IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Utils;
using IAVH.BioTablero.CM.Application.Interfaces.General;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Initiative User dto.
/// </summary>
public class InitiativeUserDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public int? InitiativeId { get; set; }

    /// <summary>
    /// User identifier.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Level relationship.
    /// </summary>
    public EnumEntityDto<InitiativeUserLevel> Level { get; set; }

    /// <summary>
    /// User focus area.
    /// </summary>
    public string FocusArea { get; set; }

    /// <summary>
    /// Entity creation date.
    /// </summary>
    public DateTime? CreationDate { get; set; }
}
