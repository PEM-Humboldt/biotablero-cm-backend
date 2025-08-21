namespace IAVH.BioTablero.CM.Application.Specifications;

using System.Linq;

using Ardalis.Specification;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Initiative Tag initiative ardalis specifications.
/// </summary>
public class InitiativeTagInitiativeSpec : GeneralSpecification<int, InitiativeTagInitiative>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public InitiativeTagInitiativeSpec()
        : base()
    {
    }

    /// <summary>
    /// Specification for get element by identifier.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    public InitiativeTagInitiativeSpec(int id)
        : base(id)
    {
    }

    /// <summary>
    /// Specification for get duplicated entities.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="tagId">Tag identifier.</param>
    /// <returns>Custom specification.</returns>
    public static InitiativeTagInitiativeSpec GetDuplicatesSpec(int initiativeId, int tagId)
    {
        var spec = new InitiativeTagInitiativeSpec();
        spec.Query
            .Where(e => e.InitiativeId == initiativeId && e.TagId == tagId);

        return spec;
    }
}
