namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.JoinRequest;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;

using IAVH.BioTablero.CM.WebApi.Interfaces;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Join Request response example.
/// </summary>
public class JoinRequestResponseExample : IOpenApiExampleProvider<JoinRequestDto>
{
    /// <inheritdoc/>
    public JoinRequestDto GetExamples() => new()
    {
        Id = 0,
        InitiativeId = 0,
        UserName = "initiative-leader@example.com",
        ReviewerUserName = "ReviewerExample",
        CreationDate = DateTimeOffset.UtcNow,
        ResponseDate = DateTimeOffset.UtcNow,
        Status = new EnumEntityDto<JoinRequestStatus>(JoinRequestStatus.UnderReview),
    };
}
