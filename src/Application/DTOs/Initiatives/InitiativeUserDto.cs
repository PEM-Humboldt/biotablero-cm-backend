namespace IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using System;
using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.DTOs.Users;
using IAVH.BioTablero.CM.Application.DTOs.Utils;
using IAVH.BioTablero.CM.Application.Interfaces.General;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Initiative User dto.
/// </summary>
[method: SetsRequiredMembers]
public class InitiativeUserDto() : IDto
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
    public required string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Level relationship.
    /// </summary>
    public required EnumEntityDto<InitiativeUserLevel> Level { get; set; } = new(InitiativeUserLevel.Reader);

    /// <summary>
    /// User focus area.
    /// </summary>
    public string? FocusArea { get; set; }

    /// <summary>
    /// Entity creation date.
    /// </summary>
    public DateTimeOffset? CreationDate { get; set; }

    /// <summary>
    /// External user data.
    /// </summary>
    public ExternalUserBaseDto? ExternalData { get; set; }
}
