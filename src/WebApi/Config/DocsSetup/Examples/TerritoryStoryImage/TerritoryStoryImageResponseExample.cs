namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.TerritoryStoryImage;

using System;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Territory Story Image response example.
/// </summary>
public class TerritoryStoryImageResponseExample : IExamplesProvider<TerritoryStoryImageDto>
{
    /// <inheritdoc/>
    public TerritoryStoryImageDto GetExamples() => new()
    {
        Id = 0,
        FeaturedContent = false,
        Description = "Territory Story Image example",
        FileUrl = new Uri("https://cdn.prod.website-files.com/64bea006689ebc2d5d9499ad/67ac933f9b2b1323a3e61073_30-humboldt-negro.svg"),
    };
}
