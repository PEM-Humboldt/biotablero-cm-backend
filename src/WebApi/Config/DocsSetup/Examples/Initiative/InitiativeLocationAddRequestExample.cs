namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Initiative Location add request example.
/// </summary>
public class InitiativeLocationAddRequestExample : IExamplesProvider<InitiativeLocationDto>
{
    /// <summary>
    /// Get examples for entity.
    /// </summary>
    /// <returns>Entity examples.</returns>
    public InitiativeLocationDto GetExamples() => new()
    {
        InitiativeId = 1,
        LocationId = 1,
        Locality = "Locality example",
    };
}
