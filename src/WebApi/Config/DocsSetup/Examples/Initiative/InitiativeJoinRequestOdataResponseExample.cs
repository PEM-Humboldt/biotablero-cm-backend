namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;

using Swashbuckle.AspNetCore.Filters;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Initiative Join Request OData response example.
/// </summary>
public class InitiativeJoinRequestOdataResponseExample : IExamplesProvider<Dictionary<string, object>>
{
    /// <summary>
    /// Get examples for entity.
    /// </summary>
    /// <returns>Entity examples.</returns>
    public Dictionary<string, object> GetExamples() => new()
    {
        ["@odata.count"] = 1,
        ["value"] = new List<InitiativeJoinRequestDto>()
        {
            new()
            {
                Id = 0,
                UserName = "Example",
                ReviewerUserName = "ReviewerExample",
                CreationDate = DateTime.Now,
                ResponseDate = DateTime.Now,
                Status = new EnumEntityDto<InitiativeJoinRequestStatus>(InitiativeJoinRequestStatus.Rejected),
            },
        },
    };
}
