namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.TerritoryStory;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Territory Story list response example.
/// </summary>
public class TerritoryStoryListResponseExample : IExamplesProvider<List<TerritoryStoryDto>>
{
    /// <summary>
    /// Get examples for entity.
    /// </summary>
    /// <returns>Entity examples.</returns>
    public List<TerritoryStoryDto> GetExamples() =>
    [
        new()
        {
            Id = 0,
            InitiativeId = 0,
            Title = "Territory Story example",
            Text = "Territory Story text example",
            Restricted = false,
            Enabled = true,
            FeaturedContent = false,
            Likes = 0,
        },
    ];
}
