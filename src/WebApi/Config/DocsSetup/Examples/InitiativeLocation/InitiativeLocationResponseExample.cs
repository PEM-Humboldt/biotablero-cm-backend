namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.InitiativeLocation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Initiative Location response example.
/// </summary>
public class InitiativeLocationResponseExample : IExamplesProvider<InitiativeLocationDto>
{
    /// <summary>
    /// Get examples for entity.
    /// </summary>
    /// <returns>Entity examples.</returns>
    public InitiativeLocationDto GetExamples() => new()
    {
        Id = 0,
        InitiativeId = 0,
        LocationId = 0,
        Locality = "Locality example",
    };
}
