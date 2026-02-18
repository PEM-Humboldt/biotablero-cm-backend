namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.ResourceLink;

using IAVH.BioTablero.CM.Application.DTOs.Resources;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Resource link response example.
/// </summary>
public class ResourceLinkResponseExample : IExamplesProvider<ResourceLinkDto>
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
