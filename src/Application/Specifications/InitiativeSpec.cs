namespace IAVH.BioTablero.CM.Application.Specifications;

using Ardalis.Specification;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Initiative ardalis specifications
/// </summary>
public class InitiativeSpec : GeneralSpecification<int, Initiative>
{
    /// <summary>
    /// Specification for get element by identifier
    /// </summary>
    /// <param name="id">Element identifier</param>
    public InitiativeSpec(int id)
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
    public InitiativeSpec(int skip, int take)
        : base(skip, take)
    {
        Query
            .Skip(skip)
            .Take(take);
    }
}
