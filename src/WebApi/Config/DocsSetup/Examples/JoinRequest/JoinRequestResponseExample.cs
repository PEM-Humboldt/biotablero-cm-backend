namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.JoinRequest;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;

using Swashbuckle.AspNetCore.Filters;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Join Request response example.
/// </summary>
public class JoinRequestResponseExample : IExamplesProvider<JoinRequestDto>
{
    /// <summary>
    /// Get examples for entity.
    /// </summary>
    /// <returns>Entity examples.</returns>
    public JoinRequestDto GetExamples() => new()
    {
        Id = 0,
        InitiativeId = 0,
        UserName = "Example",
        ReviewerUserName = "ReviewerExample",
        CreationDate = DateTime.Now,
        ResponseDate = DateTime.Now,
        Status = new EnumEntityDto<JoinRequestStatus>(JoinRequestStatus.UnderReview),
    };
}
