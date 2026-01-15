namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.ResourceType;

using IAVH.BioTablero.CM.Application.DTOs.Resources;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Resource type response example.
/// </summary>
public class ResourceTypeResponseExample : IExamplesProvider<ResourceTypeDto>
{
    /// <inheritdoc/>
    public ResourceTypeDto GetExamples() => new()
    {
        Id = 0,
        Name = "Resource type example",
        Description = "example",
    };
}
