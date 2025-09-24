namespace IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Utils;

using IAVH.BioTablero.CM.Application.Interfaces.General;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Join Request dto.
/// </summary>
public class JoinRequestDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Join Request user name.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Join Request reviewer user name.
    /// </summary>
    public string ReviewerUserName { get; set; }

    /// <summary>
    /// Join Request creation date.
    /// </summary>
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// Join Request creation date.
    /// </summary>
    public DateTime? ResponseDate { get; set; }

    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public int InitiativeId { get; set; }

    /// <summary>
    /// Join Request Status relationship.
    /// </summary>
    public EnumEntityDto<JoinRequestStatus> Status { get; set; }
}
