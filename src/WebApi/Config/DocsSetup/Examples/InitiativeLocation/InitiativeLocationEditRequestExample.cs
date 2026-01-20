namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.InitiativeLocation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Initiative Location edit request example.
/// </summary>
public class InitiativeLocationEditRequestExample : IExamplesProvider<InitiativeLocationDto>
{
    /// <inheritdoc/>
    public InitiativeLocationDto GetExamples() => new()
    {
        LocationId = 1,
        Locality = "Locality example (edited)",
    };
}
