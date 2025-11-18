namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.TerritoryStoryVideo;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Territory Story Video add request example.
/// </summary>
public class TerritoryStoryVideoAddRequestExample : IExamplesProvider<TerritoryStoryVideoDto>
{
    /// <summary>
    /// Get examples for entity.
    /// </summary>
    /// <returns>Entity examples.</returns>
    public TerritoryStoryVideoDto GetExamples() => new()
    {
        TerritoryStoryId = 1,
        FileUrl = "https://www.youtube.com/watch?v=Dhu8ee6Dzx4",
    };
}
