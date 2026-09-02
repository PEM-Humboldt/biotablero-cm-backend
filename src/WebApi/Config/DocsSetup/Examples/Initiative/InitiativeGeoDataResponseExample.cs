namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using IAVH.BioTablero.CM.WebApi.Interfaces;

/// <summary>
/// Initiative geo data response example.
/// </summary>
public class InitiativeGeoDataResponseExample : IOpenApiExampleProvider<List<InitiativeDto>>
{
    /// <inheritdoc/>
    public List<InitiativeDto> GetExamples() => [
        new()
        {
            Id = 1,
            Name = "Initiative example",
            CreationDate = DateTime.Now,
            Coordinate = [4.645238678888821, -74.09423914807002],
            Locations = [
                new()
                {
                    Id = 0,
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
                },
            ],
        },
    ];
}
