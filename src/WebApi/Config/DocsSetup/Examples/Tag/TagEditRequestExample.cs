namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Tag;

using IAVH.BioTablero.CM.Application.DTOs.Tags;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Tag edit request example.
/// </summary>
public class TagEditRequestExample : IExamplesProvider<TagDto>
{
    /// <summary>
    /// Get examples for entity.
    /// </summary>
    /// <returns>Entity examples.</returns>
    public TagDto GetExamples() => new()
    {
        Name = "Tag example",
        Url = "https://example.com/tag-data",
    };
}
