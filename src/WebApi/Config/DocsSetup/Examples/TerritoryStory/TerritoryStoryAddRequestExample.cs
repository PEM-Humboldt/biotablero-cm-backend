namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.TerritoryStory;

using System;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Initiative Contact add request example.
/// </summary>
public class TerritoryStoryAddRequestExample : IExamplesProvider<TerritoryStoryDto>
{
    /// <summary>
    /// Get examples for entity.
    /// </summary>
    /// <returns>Entity examples.</returns>
    public TerritoryStoryDto GetExamples() => new()
    {
        InitiativeId = 1,
        Title = "Territory Story example",
        Text = "Territory Story text example",
        Keywords = "Champiñón,Guanábana,Ñame",
        Restricted = false,
        Images =
        [
            new()
            {
                FileUrl = new Uri("/url/example"),
                Description = "Territory image example",
                FeaturedContent = false,
            },
        ],
        Videos =
        [
            new()
            {
                FileUrl = "/url/example",
            },
        ],
    };
}
