namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.TerritoryStory;

using System;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Territory Story response example.
/// </summary>
public class TerritoryStoryResponseExample : IExamplesProvider<TerritoryStoryDto>
{
    /// <inheritdoc/>
    public TerritoryStoryDto GetExamples() => new()
    {
        Id = 0,
        InitiativeId = 0,
        AuthorUserName = "example@example.com",
        Title = "Territory Story example",
        Text = "Territory Story text example",
        Keywords = "Champiñón,Guanábana,Ñame",
        Restricted = false,
        Enabled = true,
        FeaturedContent = false,
        Likes = 0,
        ILikedIt = false,
        Images =
        [
            new()
            {
                Id = 0,
                FileUrl = new Uri("/url/example"),
                Description = "Territory image example",
                FeaturedContent = false,
            },
        ],
        Videos =
        [
            new()
            {
                Id = 0,
                FileUrl = "https://www.youtube.com/watch?v=I2Rz6cHdoHY",
            },
        ],
    };
}
