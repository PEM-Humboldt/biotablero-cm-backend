namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Location;

using IAVH.BioTablero.CM.Application.DTOs.Geo;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

/// <summary>
/// Location response example.
/// </summary>
public class LocationResponseExample : IOpenApiExampleProvider<LocationDto>
{
    /// <inheritdoc/>
    public LocationDto GetExamples() => new()
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
    };
}
