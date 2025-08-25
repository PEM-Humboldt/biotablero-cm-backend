namespace IAVH.BioTablero.CM.Application.Specifications;

using Ardalis.Specification;

using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;

/// <summary>
/// Location Polygon custom specifications (for simplified geometries).
/// </summary>
public class LocationPolygonSimplifiedSpec : Specification<LocationPolygon, string>
{
    /// <summary>
    /// Specification for get elements by location.
    /// </summary>
    /// <param name="locationId">Location identifier.</param>
    /// <returns>Custom specification.</returns>
    public LocationPolygonSimplifiedSpec(int locationId)
    {
        Query
            .Select(e => e.GeometrySimplified)
            .Where(e => e.LocationId == locationId);
    }
}
