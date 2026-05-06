namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.JoinRequest;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;

using Swashbuckle.AspNetCore.Filters;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Join Request list response example.
/// </summary>
public class JoinRequestListResponseExample : IExamplesProvider<List<JoinRequestDto>>
{
    /// <inheritdoc/>
    public List<JoinRequestDto> GetExamples() =>
    [
        new()
        {
            Id = 0,
            InitiativeId = 0,
            UserName = "initiative-user@example.com",
            ReviewerUserName = "initiative-leader@example.com",
            CreationDate = DateTime.UtcNow,
            ResponseDate = DateTime.UtcNow,
            Status = new EnumEntityDto<JoinRequestStatus>(JoinRequestStatus.Rejected),
        }
    ];
}
