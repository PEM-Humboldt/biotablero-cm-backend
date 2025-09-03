namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Location;

using IAVH.BioTablero.CM.Application.DTOs.Geo;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Location response example.
/// </summary>
public class LocationResponseExample : IExamplesProvider<LocationDto>
{
    /// <summary>
    /// Get examples for entity.
    /// </summary>
    /// <returns>Entity examples.</returns>
    public LocationDto GetExamples() => new()
    {
        Id = 0,
        Name = "Example",
        Code = "000",
        ParentId = -1,
    };
}
