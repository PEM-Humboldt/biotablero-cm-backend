namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.ResourceFile;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Resources;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

/// <summary>
/// Resource file list response example.
/// </summary>
public class ResourceFileListResponseExample : IOpenApiExampleProvider<List<ResourceFileDto>>
{
    /// <inheritdoc/>
    public List<ResourceFileDto> GetExamples() =>
    [
        new()
        {
            Id = 0,
            ResourceId = 0,
            Name = "Resource file example",
            Url = new Uri("https://cdn.prod.website-files.com/64bea006689ebc2d5d9499ad/67ac933f9b2b1323a3e61073_30-humboldt-negro.svg"),
        },
    ];
}
