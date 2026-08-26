namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.ResourceType;

using IAVH.BioTablero.CM.Application.DTOs.Resources;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

/// <summary>
/// Resource type response example.
/// </summary>
public class ResourceTypeResponseExample : IOpenApiExampleProvider<ResourceTypeDto>
{
    /// <inheritdoc/>
    public ResourceTypeDto GetExamples() => new()
    {
        Id = 0,
        Name = "Resource type example",
        Description = "example",
    };
}
