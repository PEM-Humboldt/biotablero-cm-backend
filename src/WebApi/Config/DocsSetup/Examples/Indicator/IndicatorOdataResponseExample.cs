namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Indicator;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;

/// <summary>
/// Indicator OData response example.
/// </summary>
public class IndicatorOdataResponseExample : BaseOdataResponseExample<IndicatorDto>
{
    /// <inheritdoc/>
    protected override IndicatorDto CreateExampleDto() => new()
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
