namespace IAVH.BioTablero.CM.Application.Specifications;

using Ardalis.Specification;

using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;

/// <summary>
/// Location Polygon ardalis specifications.
/// </summary>
public class LocationPolygonSpec : GeneralSpecification<int, LocationPolygon>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public LocationPolygonSpec()
        : base()
    {
    }

    /// <summary>
    /// Specification for get element by identifier.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    public LocationPolygonSpec(int id)
        : base(id)
    {
    }

    /// <summary>
    /// Specification for get elements by location.
    /// </summary>
    /// <param name="locationId">Location identifier.</param>
    /// <returns>Custom specification.</returns>
    public static LocationPolygonSpec LocationIdSpec(int locationId)
    {
        var spec = new LocationPolygonSpec();
        spec.Query
            .Where(e => e.LocationId == locationId);

        return spec;
    }
}
