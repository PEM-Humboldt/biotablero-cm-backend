namespace IAVH.BioTablero.CM.Application.Specifications;

using System.Linq;

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
                    .ThenInclude(e => e.Parent)
            .Include(e => e.InitiativeTags)
                .ThenInclude(e => e.Tag);
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

    /// <summary>
    /// Specification for get elements by tag.
    /// </summary>
    /// <param name="id">Tag identifier.</param>
    /// <returns>Custom specification.</returns>
    public static InitiativeSpec GetByTagSpec(int id)
    {
        var spec = new InitiativeSpec();
        spec.Query
            .Where(e => e.InitiativeTags
                .Any(e => e.TagId == id));

        return spec;
    }

    /// <summary>
    /// Specification for get elements by user name.
    /// </summary>
    /// <param name="userName">Usre name.</param>
    /// <returns>Custom specification.</returns>
    public static InitiativeSpec UserNameSpec(string userName)
    {
        var spec = new InitiativeSpec();
        spec.Query
            .Where(e => e.InitiativeUsers
                .Any(e => e.UserName == userName))
            .Include(e => e.InitiativeUsers);

        return spec;
    }
}
