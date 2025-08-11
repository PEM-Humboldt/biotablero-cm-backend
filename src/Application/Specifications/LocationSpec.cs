namespace IAVH.BioTablero.CM.Application.Specifications;

using Ardalis.Specification;

using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;

/// <summary>
/// Location ardalis specifications.
/// </summary>
public class LocationSpec : GeneralSpecification<int, Location>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public LocationSpec()
        : base()
    {
    }

    /// <summary>
    /// Specification for get element by identifier.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    public LocationSpec(int id)
        : base(id)
    {
    }

    /// <summary>
    /// Specification for get paginated elements.
    /// </summary>
    /// <param name="skip">Page number.</param>
    /// <param name="take">Page size.</param>
    public LocationSpec(int skip, int take)
        : base(skip, take)
    {
    }

    /// <summary>
    /// Specification for get elements by parent.
    /// </summary>
    /// <param name="parentId">Parent identifier.</param>
    /// <returns>Custom specification.</returns>
    public static LocationSpec ParentIdSpec(int? parentId)
    {
        var spec = new LocationSpec();
        spec.Query
            .Where(e => e.ParentId == parentId);

        return spec;
    }
}
