namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Location;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Geo;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

/// <summary>
/// Location list response example.
/// </summary>
public class LocationListResponseExample : IOpenApiExampleProvider<List<LocationDto>>
{
    /// <inheritdoc/>
    public List<LocationDto> GetExamples() =>
    [
        new()
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
        }
    ];
}
