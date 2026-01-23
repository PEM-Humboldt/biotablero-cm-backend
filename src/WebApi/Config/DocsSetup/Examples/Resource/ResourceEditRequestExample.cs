namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Resource;

using IAVH.BioTablero.CM.Application.DTOs.Resources;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Resource edit request example.
/// </summary>
public class ResourceEditRequestExample : IExamplesProvider<ResourceDto>
{
    /// <inheritdoc/>
    public ResourceDto GetExamples() => new()
    {
        ResourceType = new()
        {
            Id = 1,
        },
        Name = "Resource example",
        Description = "Example",
        IsDraft = false,
    };
}
