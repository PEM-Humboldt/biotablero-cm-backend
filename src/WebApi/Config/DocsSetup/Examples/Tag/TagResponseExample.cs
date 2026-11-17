namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Tag;

using IAVH.BioTablero.CM.Application.DTOs.Tags;
using IAVH.BioTablero.CM.Application.DTOs.Utils;

using IAVH.BioTablero.CM.WebApi.Interfaces;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.TagEnums;

/// <summary>
/// Tag response example.
/// </summary>
public class TagResponseExample : IOpenApiExampleProvider<TagDto>
{
    /// <inheritdoc/>
    public TagDto GetExamples() => new()
    {
        Id = 0,
        Name = "Tag example",
        FullName = "Full name example",
        Url = "https://example.com/tag-data",
        Category = new EnumEntityDto<TagCategory>(TagCategory.PoliticalContext),
    };
}
