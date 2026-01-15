namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.ResourceFile;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Resources;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Resource file response example.
/// </summary>
public class ResourceFileResponseExample : IExamplesProvider<ResourceFileDto>
{
    /// <inheritdoc/>
    public ResourceFileDto GetExamples() => new()
    {
        Id = 0,
        ResourceId = 0,
        Name = "Resource link example",
        Url = new Uri("https://cdn.prod.website-files.com/64bea006689ebc2d5d9499ad/67ac933f9b2b1323a3e61073_30-humboldt-negro.svg"),
    };
}
