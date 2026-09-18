namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Observation;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;

/// <summary>
/// Observation OData response example.
/// </summary>
public class ObservationOdataResponseExample : BaseOdataResponseExample<ObservationDto>
{
    /// <inheritdoc/>
    protected override ObservationDto CreateExampleDto() => new()
    {
        Id = 0,
        Name = "Observation example",
        InitiativeId = 0,
        Topic = new()
        {
            Id = 0,
            Name = "Indicator topic example",
        },
        Versions = [
            new()
            {
                Id = 0,
                Version = 0,
                CreationDate = DateTime.Now,
            }
        ],
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
        Tags = [
            new()
            {
                ObservationTagId = 0,
                Tag = new()
                {
                    Id = 0,
                    Name = "Tag example",
                },
            }
        ],
    };
}
