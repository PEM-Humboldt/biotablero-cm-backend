namespace IAVH.BioTablero.CM.Application.Specifications;

using System.Linq;

using Ardalis.Specification;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Initiative Tag ardalis specifications.
/// </summary>
public class InitiativeTagSpec : GeneralSpecification<int, InitiativeTag>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public InitiativeTagSpec()
        : base()
    {
    }

    /// <summary>
    /// Specification for get element by identifier.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    public InitiativeTagSpec(int id)
        : base(id)
    {
    }

    /// <summary>
    /// Specification for get duplicated entities.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="tagId">Tag identifier.</param>
    /// <returns>Custom specification.</returns>
    public static InitiativeTagSpec GetDuplicatesSpec(int initiativeId, int tagId)
    {
        var spec = new InitiativeTagSpec();
        spec.Query
            .Where(e => e.InitiativeId == initiativeId && e.TagId == tagId);

        return spec;
    }
}
