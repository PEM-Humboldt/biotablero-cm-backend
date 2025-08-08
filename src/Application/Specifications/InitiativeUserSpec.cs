namespace IAVH.BioTablero.CM.Application.Specifications;

using Ardalis.Specification;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Initiative User ardalis specifications.
/// </summary>
public class InitiativeUserSpec : GeneralSpecification<int, InitiativeUser>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public InitiativeUserSpec()
    {
    }

    /// <summary>
    /// Specification for get element by identifier.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    public InitiativeUserSpec(int id)
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
    public InitiativeUserSpec(int skip, int take)
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
    public static InitiativeUserSpec InitiativeIdSpec(int initiativeId)
    {
        var spec = new InitiativeUserSpec();
        spec.Query
            .Where(e => e.InitiativeId == initiativeId);

        return spec;
    }

    /// <summary>
    /// Specification for get duplicated entities.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="userName">User name.</param>
    /// <returns>Custom specification.</returns>
    public static InitiativeUserSpec UserNameSpec(int initiativeId, string userName)
    {
        var spec = new InitiativeUserSpec();
        spec.Query
            .Where(e => e.InitiativeId == initiativeId && e.UserName == userName);

        return spec;
    }

    /// <summary>
    /// Specification for get entities by level.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="levelId">Level identifier.</param>
    /// <returns>Custom specification.</returns>
    public static InitiativeUserSpec LevelSpec(int initiativeId, int levelId)
    {
        var spec = new InitiativeUserSpec();
        spec.Query
            .Where(e => e.InitiativeId == initiativeId && e.LevelId == levelId);

        return spec;
    }

    /// <summary>
    /// Specification for get entities by level.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="levelId">Level identifier.</param>
    /// <returns>Custom specification.</returns>
    public static InitiativeUserSpec LevelSpec(int id, int initiativeId, int levelId)
    {
        var spec = new InitiativeUserSpec();
        spec.Query
            .Where(e => e.Id != id && e.InitiativeId == initiativeId && e.LevelId == levelId);

        return spec;
    }
}
