namespace IAVH.BioTablero.CM.Application.Specifications;

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
    /// Specification for get elements by name.
    /// </summary>
    /// <param name="name">Entity name.</param>
    /// <returns>Custom specification.</returns>
    public InitiativeTagSpec(string name)
    {
        Query
            .Where(e => e.Name == name);
    }

    /// <summary>
    /// Specification for get elements by name.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="name">Entity name.</param>
    /// <returns>Custom specification.</returns>
    public InitiativeTagSpec(int id, string name)
    {
        Query
            .Where(e => e.Id != id && e.Name == name);
    }
}
