namespace IAVH.BioTablero.CM.Application.Specifications;

using Ardalis.Specification;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Initiative Tag ardalis specifications.
/// </summary>
public class TagSpec : GeneralSpecification<int, Tag>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public TagSpec()
        : base()
    {
    }

    /// <summary>
    /// Specification for get element by identifier.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    public TagSpec(int id)
        : base(id)
    {
    }

    /// <summary>
    /// Specification for get elements by name.
    /// </summary>
    /// <param name="name">Entity name.</param>
    /// <returns>Custom specification.</returns>
    public TagSpec(string name)
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
    public TagSpec(int id, string name)
    {
        Query
            .Where(e => e.Id != id && e.Name == name);
    }
}
