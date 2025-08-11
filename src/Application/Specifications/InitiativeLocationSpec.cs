namespace IAVH.BioTablero.CM.Application.Specifications;

using Ardalis.Specification;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Initiative Location ardalis specifications.
/// </summary>
public class InitiativeLocationSpec : GeneralSpecification<int, InitiativeLocation>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public InitiativeLocationSpec()
    {
    }

    /// <summary>
    /// Specification for get element by identifier.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    public InitiativeLocationSpec(int id)
        : base(id)
    {
        Query
            .Where(e => e.Id == id);
    }

    /// <summary>
    /// Specification for get paginated elements.
    /// </summary>
    /// <param name="skip">Page number.</param>
    /// <param name="take">Page size.</param>
    public InitiativeLocationSpec(int skip, int take)
        : base(skip, take)
    {
        Query
            .Skip(skip)
            .Take(take);
    }

    /// <summary>
    /// Specification for get elements by initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <returns>Custom specification.</returns>
    public static InitiativeLocationSpec InitiativeIdSpec(int initiativeId)
    {
        var spec = new InitiativeLocationSpec();
        spec.Query
            .Where(e => e.InitiativeId == initiativeId);

        return spec;
    }

    /// <summary>
    /// Specification for get duplicated entities.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="locationId">Location identifier.</param>
    /// <param name="locality">Locality name.</param>
    /// <returns>Custom specification.</returns>
    public static InitiativeLocationSpec LocationIdAndLocalitySpec(int initiativeId, int locationId, string locality)
    {
        var spec = new InitiativeLocationSpec();
        spec.Query
            .Where(e => e.InitiativeId == initiativeId && e.LocationId == locationId && e.Locality == locality);

        return spec;
    }

    /// <summary>
    /// Specification for get duplicated entities.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="locationId">Location identifier.</param>
    /// <param name="locality">Locality name.</param>
    /// <returns>Custom specification.</returns>
    public static InitiativeLocationSpec LocationIdAndLocalitySpec(int id, int initiativeId, int locationId, string locality)
    {
        var spec = new InitiativeLocationSpec();
        spec.Query
            .Where(e => e.Id != id && e.InitiativeId == initiativeId && e.LocationId == locationId && e.Locality == locality);

        return spec;
    }
}
