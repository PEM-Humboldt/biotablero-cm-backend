namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Tag;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;

using Swashbuckle.AspNetCore.Filters;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Tag add request example.
/// </summary>
public class TagAddRequestExample : IExamplesProvider<TagDto>
{
    /// <summary>
    /// Get examples for entity.
    /// </summary>
    /// <returns>Entity examples.</returns>
    public TagDto GetExamples() => new()
    {
        Name = "Tag example",
        Url = "https://example.com/tag-data",
        Category = new EnumEntityDto<TagCategory>(TagCategory.PoliticalContext),
    };
}
