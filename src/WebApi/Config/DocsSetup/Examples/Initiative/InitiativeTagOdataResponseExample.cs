namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;

using Swashbuckle.AspNetCore.Filters;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Initiative Tag OData response example.
/// </summary>
public class InitiativeTagOdataResponseExample : IExamplesProvider<Dictionary<string, object>>
{
    /// <summary>
    /// Get examples for entity.
    /// </summary>
    /// <returns>Entity examples.</returns>
    public Dictionary<string, object> GetExamples() => new()
    {
        ["@odata.count"] = 1,
        ["value"] = new List<InitiativeTagDto>()
        {
            new()
            {
                Id = 0,
                Name = "Initiative example",
                Url = new Uri("https://example.com/tag-data"),
                Category = new EnumEntityDto<InitiativeTagCategory>(InitiativeTagCategory.PoliticalContext),
            },
        },
    };
}
