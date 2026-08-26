namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.ResourceLink;

using IAVH.BioTablero.CM.Application.DTOs.Resources;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

/// <summary>
/// Resource link response example.
/// </summary>
public class ResourceLinkResponseExample : IOpenApiExampleProvider<ResourceLinkDto>
{
    /// <inheritdoc/>
    public ResourceLinkDto GetExamples() => new()
    {
        Id = 0,
        ResourceId = 0,
        Name = "Resource link example",
        Url = "http://example.com",
    };
}
