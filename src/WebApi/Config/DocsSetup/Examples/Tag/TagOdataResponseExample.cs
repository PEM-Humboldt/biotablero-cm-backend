namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Tag;

using IAVH.BioTablero.CM.Application.DTOs.Tags;
using IAVH.BioTablero.CM.Application.DTOs.Utils;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.TagEnums;

/// <summary>
/// Tag OData response example.
/// </summary>
public class TagOdataResponseExample : BaseOdataResponseExample<TagDto>
{
    /// <inheritdoc/>
    protected override TagDto CreateExampleDto() => new()
    {
        Id = 0,
        Name = "Tag example",
        Url = "https://example.com/tag-data",
        Category = new EnumEntityDto<TagCategory>(TagCategory.PoliticalContext),
    };
}
