namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Resource;

using IAVH.BioTablero.CM.Application.DTOs.Resources;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Resource response example.
/// </summary>
public class ResourceResponseExample : IExamplesProvider<ResourceDto>
{
    /// <inheritdoc/>
    public ResourceDto GetExamples() => new()
    {
        Id = 0,
        InitiativeId = 0,
        ResourceType = new()
        {
            Id = 0,
            Name = "Resource type example",
        },
        Name = "Resource example",
        Description = "Example",
        IsDraft = false,
        Likes = 0,
        ILikedIt = false,
    };
}
