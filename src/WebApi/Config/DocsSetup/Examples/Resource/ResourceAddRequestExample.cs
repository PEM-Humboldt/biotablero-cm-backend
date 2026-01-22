namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Resource;

using IAVH.BioTablero.CM.Application.DTOs.Resources;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Resource add request example.
/// </summary>
public class ResourceAddRequestExample : IExamplesProvider<ResourceDto>
{
    /// <inheritdoc/>
    public ResourceDto GetExamples() => new()
    {
        InitiativeId = 1,
        Name = "Resource example",
        Description = "Example",
        IsDraft = false,
    };
}
