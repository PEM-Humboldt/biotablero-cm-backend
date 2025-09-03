namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Location;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Geo;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Location list response example.
/// </summary>
public class LocationListResponseExample : IExamplesProvider<List<LocationDto>>
{
    /// <summary>
    /// Get examples for entity.
    /// </summary>
    /// <returns>Entity examples.</returns>
    public List<LocationDto> GetExamples() =>
    [
        new()
        {
            Id = 0,
            Name = "Example",
            Code = "000",
            ParentId = -1,
        }
    ];
}
