namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;

using Swashbuckle.AspNetCore.Filters;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Initiative Tag response example.
/// </summary>
public class InitiativeTagResponseExample : IExamplesProvider<InitiativeTagDto>
{
    /// <summary>
    /// Get examples for entity.
    /// </summary>
    /// <returns>Entity examples.</returns>
    public InitiativeTagDto GetExamples() => new()
    {
        Id = 0,
        Name = "Tag example",
        Url = new Uri("https://example.com/tag-data"),
        Category = new EnumEntityDto<InitiativeTagCategory>(InitiativeTagCategory.PoliticalContext),
    };
}
