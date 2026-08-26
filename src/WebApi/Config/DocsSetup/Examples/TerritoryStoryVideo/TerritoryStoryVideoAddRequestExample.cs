namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.TerritoryStoryVideo;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;

using IAVH.BioTablero.CM.WebApi.Interfaces;

/// <summary>
/// Territory Story Video add request example.
/// </summary>
public class TerritoryStoryVideoAddRequestExample : IOpenApiExampleProvider<TerritoryStoryVideoDto>
{
    /// <inheritdoc/>
    public TerritoryStoryVideoDto GetExamples() => new()
    {
        TerritoryStoryId = 1,
        FileUrl = "https://www.youtube.com/watch?v=I2Rz6cHdoHY",
    };
}
