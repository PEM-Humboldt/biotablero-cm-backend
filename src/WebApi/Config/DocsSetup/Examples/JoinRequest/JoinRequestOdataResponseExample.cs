namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.JoinRequest;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Join Request OData response example.
/// </summary>
public class JoinRequestOdataResponseExample : BaseOdataResponseExample<JoinRequestDto>
{
    /// <inheritdoc/>
    protected override JoinRequestDto CreateExampleDto() => new()
    {
        Id = 0,
        InitiativeId = 0,
        UserName = "initiative-user@example.com",
        ReviewerUserName = "initiative-leader@example.com",
        CreationDate = DateTime.UtcNow,
        ResponseDate = DateTime.UtcNow,
        Status = new EnumEntityDto<JoinRequestStatus>(JoinRequestStatus.Rejected),
    };
}
