namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Indicator;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;

using IAVH.BioTablero.CM.WebApi.Interfaces;

/// <summary>
/// Indicator response example.
/// </summary>
public class IndicatorResponseExample : IOpenApiExampleProvider<IndicatorDto>
{
    /// <inheritdoc/>
    public IndicatorDto GetExamples() => new()
    {
        Id = 0,
        Name = "Indicator example",
        InitiativeId = 0,
        Type = new()
        {
            Id = 0,
            Name = "Indicator type example",
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
                IndicatorTagId = 0,
                Tag = new()
                {
                    Id = 0,
                    Name = "Tag example",
                },
            }
        ],
    };
}
