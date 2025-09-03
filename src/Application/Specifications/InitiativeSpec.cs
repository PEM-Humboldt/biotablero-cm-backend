namespace IAVH.BioTablero.CM.Application.Specifications;

using Ardalis.Specification;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Initiative ardalis specifications.
/// </summary>
public class InitiativeSpec : GeneralSpecification<int, Initiative>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public InitiativeSpec()
        : base()
    {
    }

    /// <summary>
    /// Specification for get element by identifier.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    public InitiativeSpec(int id)
        : base(id)
    {
        Query
            .Where(e => e.Id == id)
            .Include(e => e.InitiativeContacts)
            .Include(e => e.InitiativeUsers)
            .Include(e => e.InitiativeLocations)
                .ThenInclude(e => e.Location)
                    .ThenInclude(e => e.Parent);
    }

    /// <summary>
    /// Specification for get element by name.
    /// </summary>
    /// <param name="name">Element name.</param>
    public InitiativeSpec(string name)
    {
        Query
            .Where(e => e.Name == name);
    }

    /// <summary>
    /// Specification for get duplicated entities.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="name">Entity name.</param>
    /// <returns>Custom specification.</returns>
    public static InitiativeSpec GetDuplicatesSpec(int id, string name)
    {
        var spec = new InitiativeSpec();
        spec.Query
            .Where(e => e.Id != id && e.Name == name);

        return spec;
    }
}
