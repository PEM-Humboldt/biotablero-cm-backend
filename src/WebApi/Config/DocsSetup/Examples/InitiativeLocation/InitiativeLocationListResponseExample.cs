namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.InitiativeLocation;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Initiative Location list response example.
/// </summary>
public class InitiativeLocationListResponseExample : IExamplesProvider<List<InitiativeLocationDto>>
{
    /// <summary>
    /// Get examples for entity.
    /// </summary>
    /// <returns>Entity examples.</returns>
    public List<InitiativeLocationDto> GetExamples() =>
    [
        new()
        {
            Id = 0,
            InitiativeId = 0,
            LocationId = 0,
            Locality = "Locality example",
        }
    ];
}
