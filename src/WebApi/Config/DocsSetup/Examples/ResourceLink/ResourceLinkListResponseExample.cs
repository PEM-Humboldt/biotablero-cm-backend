namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.ResourceLink;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Resources;

using IAVH.BioTablero.CM.WebApi.Interfaces;

/// <summary>
/// Resource Link list response example.
/// </summary>
public class ResourceLinkListResponseExample : IOpenApiExampleProvider<List<ResourceLinkDto>>
{
    /// <inheritdoc/>
    public List<ResourceLinkDto> GetExamples() =>
    [
        new()
        {
            Id = 0,
            ResourceId = 0,
            Name = "Resource link example",
            Url = "http://example.com",
        },
    ];
}
