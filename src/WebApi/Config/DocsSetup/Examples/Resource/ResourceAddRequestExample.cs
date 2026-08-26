namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Resource;

using IAVH.BioTablero.CM.Application.DTOs.Resources;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

/// <summary>
/// Resource add request example.
/// </summary>
public class ResourceAddRequestExample : IOpenApiExampleProvider<ResourceDto>
{
    /// <inheritdoc/>
    public ResourceDto GetExamples() => new()
    {
        InitiativeId = 1,
        ResourceType = new()
        {
            Id = 1,
        },
        Name = "Resource example",
        Description = "Example",
        IsDraft = false,
    };
}
