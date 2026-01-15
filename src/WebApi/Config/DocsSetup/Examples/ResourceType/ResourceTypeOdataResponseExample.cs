namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.ResourceType;

using IAVH.BioTablero.CM.Application.DTOs.Resources;

/// <summary>
/// Resource type OData response example.
/// </summary>
public class ResourceTypeOdataResponseExample : BaseOdataResponseExample<ResourceTypeDto>
{
    /// <inheritdoc/>
    protected override ResourceTypeDto CreateExampleDto() => new()
    {
        Id = 0,
        Name = "Resource type example",
        Description = "example",
    };
}
