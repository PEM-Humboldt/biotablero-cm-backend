namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.InitiativeLocation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

/// <summary>
/// Initiative Location edit request example.
/// </summary>
public class InitiativeLocationEditRequestExample : IOpenApiExampleProvider<InitiativeLocationDto>
{
    /// <inheritdoc/>
    public InitiativeLocationDto GetExamples() => new()
    {
        LocationId = 1,
        Locality = "Locality example (edited)",
    };
}
