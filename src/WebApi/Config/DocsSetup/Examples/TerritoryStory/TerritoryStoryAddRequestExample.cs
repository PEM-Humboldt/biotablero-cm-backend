namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.TerritoryStory;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Territory Story add request example.
/// </summary>
public class TerritoryStoryAddRequestExample : IExamplesProvider<TerritoryStoryDto>
{
    /// <inheritdoc/>
    public TerritoryStoryDto GetExamples() => new()
    {
        InitiativeId = 1,
        Title = "Territory Story example",
        Text = "Territory Story text example",
        Keywords = "Champiñón,Guanábana,Ñame",
        Restricted = false,
        Videos =
        [
            new()
            {
                FileUrl = "https://www.youtube.com/watch?v=I2Rz6cHdoHY",
            },
        ],
    };
}
