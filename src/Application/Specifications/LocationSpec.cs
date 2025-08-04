namespace IAVH.BioTablero.CM.Application.Specifications;

using Ardalis.Specification;

using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;

/// <summary>
/// Location ardalis specifications
/// </summary>
public class LocationSpec : GeneralSpecification<int, Location>
{
    /// <summary>
    /// Specification for get element by identifier
    /// </summary>
    /// <param name="id">Element identifier</param>
    public LocationSpec(int id)
        : base(id)
    {
        Query
            .Where(e => e.Id == id);
    }

    /// <summary>
    /// Specification for get paginated elements
    /// </summary>
    /// <param name="skip">Page number</param>
    /// <param name="take">Page size</param>
    public LocationSpec(int skip, int take)
        : base(skip, take)
    {
        Query
            .Skip(skip)
            .Take(take);
    }
}
