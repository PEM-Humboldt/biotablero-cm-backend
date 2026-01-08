namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.TerritoryStoryVideo;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Territory Story Video list response example.
/// </summary>
public class TerritoryStoryVideoListResponseExample : IExamplesProvider<List<TerritoryStoryVideoDto>>
{
    /// <inheritdoc/>
    public List<TerritoryStoryVideoDto> GetExamples() =>
    [
        new()
        {
            Id = 0,
            TerritoryStoryId = 0,
            FileUrl = "https://www.youtube.com/watch?v=I2Rz6cHdoHY",
        },
    ];
}
