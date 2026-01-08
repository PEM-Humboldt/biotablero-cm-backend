namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.InitiativeLocation;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Initiative Location list response example.
/// </summary>
public class InitiativeLocationListResponseExample : IExamplesProvider<List<InitiativeLocationDto>>
{
    /// <inheritdoc/>
    public List<InitiativeLocationDto> GetExamples() =>
    [
        new()
        {
            Id = 0,
            InitiativeId = 0,
            LocationId = 0,
            Locality = "Locality example",
            Location = new()
            {
                Id = 0,
                Name = "Example",
                Code = "000",
                Parent = new()
                {
                    Id = 0,
                    Name = "Example",
                    Code = "000",
                },
            },
        }
    ];
}
