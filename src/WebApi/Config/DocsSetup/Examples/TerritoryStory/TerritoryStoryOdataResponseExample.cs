namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.TerritoryStory;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;

/// <summary>
/// Territory Story list response example.
/// </summary>
public class TerritoryStoryOdataResponseExample : BaseOdataResponseExample<TerritoryStoryDto>
{
    /// <inheritdoc/>
    protected override TerritoryStoryDto CreateExampleDto() =>
        new()
        {
            Id = 0,
            InitiativeId = 0,
            AuthorUserName = "example@example.com",
            Title = "Territory Story example",
            Text = "Territory Story text example",
            Restricted = false,
            Enabled = true,
            FeaturedContent = false,
            Likes = 0,
            ILikedIt = false,
        };
}
