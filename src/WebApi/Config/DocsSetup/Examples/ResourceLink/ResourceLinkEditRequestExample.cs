namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.ResourceLink;

using IAVH.BioTablero.CM.Application.DTOs.Resources;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

/// <summary>
/// Resource Link add request example.
/// </summary>
public class ResourceLinkEditRequestExample : IOpenApiExampleProvider<ResourceLinkDto>
{
    /// <inheritdoc/>
    public ResourceLinkDto GetExamples() => new()
    {
        Name = "Resource link example",
        Url = "https://www.youtube.com/watch?v=I2Rz6cHdoHY",
    };
}
