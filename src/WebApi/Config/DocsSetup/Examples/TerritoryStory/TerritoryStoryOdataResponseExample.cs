namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.TerritoryStory;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;

/// <summary>
/// Initiative OData response example.
/// </summary>
public class TerritoryStoryOdataResponseExample : BaseOdataResponseExample<TerritoryStoryDto>
{
    /// <summary>
    /// Create example DTO object.
    /// </summary>
    /// <returns>Example DTO object.</returns>
    protected override TerritoryStoryDto CreateExampleDto() => new()
    {
        Id = 0,
        InitiativeId = 0,
        Title = "Territory Story example",
        Text = "Territory Story text example",
        Restricted = false,
        Enabled = true,
        FeaturedContent = false,
        Likes = 0,
    };
}
