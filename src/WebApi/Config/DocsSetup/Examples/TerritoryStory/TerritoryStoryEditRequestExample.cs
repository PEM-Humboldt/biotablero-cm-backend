namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.TerritoryStory;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;

using IAVH.BioTablero.CM.WebApi.Interfaces;

/// <summary>
/// Territory Story edit request example.
/// </summary>
public class TerritoryStoryEditRequestExample : IOpenApiExampleProvider<TerritoryStoryDto>
{
    /// <inheritdoc/>
    public TerritoryStoryDto GetExamples() => new()
    {
        Title = "Territory Story example",
        Text = "Territory Story text example",
        Keywords = "Champiñón,Guanábana,Ñame",
        Restricted = false,
    };
}
