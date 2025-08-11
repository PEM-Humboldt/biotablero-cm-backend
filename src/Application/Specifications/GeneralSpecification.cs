namespace IAVH.BioTablero.CM.Application.Specifications;

using Ardalis.Specification;

using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// General Ardalis Specification.
/// </summary>
/// <typeparam name="TI">Entity identifier type.</typeparam>
/// <typeparam name="TE">Entity type.</typeparam>
public abstract class GeneralSpecification<TI, TE> : Specification<TE>
where TI : notnull
where TE : BaseEntity<TI>, IAggregateRoot
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    protected GeneralSpecification()
    {
    }

    /// <summary>
    /// Constructor for one element query.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    protected GeneralSpecification(TI id)
    {
        Query
            .Where(e => e.Id.Equals(id));
    }

    /// <summary>
    /// Constructor for a paginated query.
    /// </summary>
    /// <param name="skip">Page number.</param>
    /// <param name="take">Page size.</param>
    protected GeneralSpecification(int skip, int take)
    {
        Query
            .Skip(skip)
            .Take(take);
    }
}
