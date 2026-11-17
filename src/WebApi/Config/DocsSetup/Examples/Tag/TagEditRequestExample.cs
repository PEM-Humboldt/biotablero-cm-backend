namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Tag;

using IAVH.BioTablero.CM.Application.DTOs.Tags;

using IAVH.BioTablero.CM.WebApi.Interfaces;

/// <summary>
/// Tag edit request example.
/// </summary>
public class TagEditRequestExample : IOpenApiExampleProvider<TagDto>
{
    /// <inheritdoc/>
    public TagDto GetExamples() => new()
    {
        Name = "Tag example",
        FullName = "Full name example",
        Url = "https://example.com/tag-data",
    };
}
