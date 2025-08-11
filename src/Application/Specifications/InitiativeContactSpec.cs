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
        : base()
    {
    }

    /// <summary>
    /// Specification for get element by identifier.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    public InitiativeContactSpec(int id)
        : base(id)
    {
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

    /// <summary>
    /// Specification for get duplicated entities (email or phone).
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="email">Contact email.</param>
    /// <param name="phone">Contact phone.</param>
    /// <returns>Custom specification.</returns>
    public static InitiativeContactSpec EmailOrPhoneSpec(int initiativeId, string email, string phone)
    {
        var spec = new InitiativeContactSpec();
        spec.Query
            .Where(e => e.InitiativeId == initiativeId && (e.Phone == phone || e.Email == email));

        return spec;
    }

    /// <summary>
    /// Specification for get duplicated entities (email or phone).
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="email">Contact email.</param>
    /// <param name="phone">Contact phone.</param>
    /// <returns>Custom specification.</returns>
    public static InitiativeContactSpec EmailOrPhoneSpec(int id, int initiativeId, string email, string phone)
    {
        var spec = new InitiativeContactSpec();
        spec.Query
            .Where(e => e.Id != id && e.InitiativeId == initiativeId && (e.Phone == phone || e.Email == email));

        return spec;
    }
}
