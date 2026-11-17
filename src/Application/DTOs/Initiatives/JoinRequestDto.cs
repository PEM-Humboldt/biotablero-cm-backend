namespace IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using System;
using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.DTOs.Utils;

using IAVH.BioTablero.CM.Application.Interfaces.General;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Join Request dto.
/// </summary>
[method: SetsRequiredMembers]
public class JoinRequestDto() : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Join Request user name.
    /// </summary>
    public required string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Join Request reviewer user name.
    /// </summary>
    public string? ReviewerUserName { get; set; }

    /// <summary>
    /// Join Request creation date.
    /// </summary>
    public DateTimeOffset? CreationDate { get; set; }

    /// <summary>
    /// Join Request creation date.
    /// </summary>
    public DateTimeOffset? ResponseDate { get; set; }

    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public int InitiativeId { get; set; }

    /// <summary>
    /// Level relationship.
    /// </summary>
    public EnumEntityDto<InitiativeUserLevel>? Level { get; set; }

    /// <summary>
    /// Join Request Status relationship.
    /// </summary>
    public required EnumEntityDto<JoinRequestStatus> Status { get; set; } = new(JoinRequestStatus.UnderReview);
}
