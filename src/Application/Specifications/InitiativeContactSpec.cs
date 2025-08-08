namespace IAVH.BioTablero.CM.Application.Specifications;

using Ardalis.Specification;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Initiative Contact ardalis specifications.
/// </summary>
public class InitiativeContactSpec : GeneralSpecification<int, InitiativeContact>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public InitiativeContactSpec()
    {
    }

    /// <summary>
    /// Specification for get element by identifier.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    public InitiativeContactSpec(int id)
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
    public InitiativeContactSpec(int skip, int take)
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
    public static InitiativeContactSpec InitiativeIdSpec(int initiativeId)
    {
        var spec = new InitiativeContactSpec();
        spec.Query
            .Where(e => e.InitiativeId == initiativeId);

        return spec;
    }
}
