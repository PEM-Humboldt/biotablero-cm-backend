namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Indicator;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Indicator response example.
/// </summary>
public class IndicatorResponseExample : IExamplesProvider<IndicatorDto>
{
    /// <inheritdoc/>
    public IndicatorDto GetExamples() => new()
    {
        Id = 0,
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
