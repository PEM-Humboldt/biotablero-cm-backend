namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Tag;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Tag OData response example.
/// </summary>
public class TagOdataResponseExample : BaseOdataResponseExample<TagDto>
{
    /// <summary>
    /// Create example DTO object.
    /// </summary>
    /// <returns>Example DTO object.</returns>
    protected override TagDto CreateExampleDto() => new()
    {
        Id = 0,
        Name = "Tag example",
        Url = "https://example.com/tag-data",
        Category = new EnumEntityDto<TagCategory>(TagCategory.PoliticalContext),
    };
}
