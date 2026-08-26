namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.TerritoryStoryVideo;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

/// <summary>
/// Territory Story Video response example.
/// </summary>
public class TerritoryStoryVideoResponseExample : IOpenApiExampleProvider<TerritoryStoryVideoDto>
{
    /// <inheritdoc/>
    public TerritoryStoryVideoDto GetExamples() => new()
    {
        Id = 0,
        TerritoryStoryId = 0,
        FileUrl = "https://www.youtube.com/watch?v=I2Rz6cHdoHY",
    };
}
