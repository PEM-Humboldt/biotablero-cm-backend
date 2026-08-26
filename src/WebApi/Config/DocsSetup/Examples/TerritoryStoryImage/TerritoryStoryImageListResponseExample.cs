namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.TerritoryStoryImage;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

/// <summary>
/// Territory Story Image list response example.
/// </summary>
public class TerritoryStoryImageListResponseExample : IOpenApiExampleProvider<List<TerritoryStoryImageDto>>
{
    /// <inheritdoc/>
    public List<TerritoryStoryImageDto> GetExamples() =>
    [
        new()
        {
            Id = 0,
            FeaturedContent = false,
            Description = "Territory Story Image example",
            FileUrl = new Uri("https://cdn.prod.website-files.com/64bea006689ebc2d5d9499ad/67ac933f9b2b1323a3e61073_30-humboldt-negro.svg"),
        },
    ];
}
