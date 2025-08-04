namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Location;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Geo;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Location OData response example
/// </summary>
public class LocationOdataResponseExample : IExamplesProvider<Dictionary<string, object>>
{
    /// <summary>
    /// Get examples for entity
    /// </summary>
    /// <returns>Entity examples</returns>
    public Dictionary<string, object> GetExamples() => new()
    {
        ["@odata.count"] = 1,
        ["value"] = new List<LocationDto>()
        {
            new(),
        },
    };
}
