namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.TerritoryStoryVideo;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;

using IAVH.BioTablero.CM.WebApi.Interfaces;

/// <summary>
/// Territory Story Video edit request example.
/// </summary>
public class TerritoryStoryVideoEditRequestExample : IOpenApiExampleProvider<TerritoryStoryVideoDto>
{
    /// <inheritdoc/>
    public TerritoryStoryVideoDto GetExamples() => new()
    {
        FileUrl = "https://www.youtube.com/watch?v=B8lUy-dJ0zY",
    };
}
