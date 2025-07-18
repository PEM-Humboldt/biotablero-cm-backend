namespace IAVH.BioTablero.CM.Application.Specifications;

using Ardalis.Specification;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// General Ardalis Specification
/// </summary>
/// <typeparam name="T">Entity identifier type</typeparam>
/// <typeparam name="E">Entity type</typeparam>
public abstract class GeneralSpecification<T, E> : Specification<E>
where T : notnull
where E : class, IAggregateRoot
{
    /// <summary>
    /// Default constructor
    /// </summary>
    protected GeneralSpecification()
    {
    }

    /// <summary>
    /// Constructor for one element query
    /// </summary>
    /// <param name="id">Element identifier</param>
    protected GeneralSpecification(T id)
    {
    }

    /// <summary>
    /// Constructor for a paginated query
    /// </summary>
    /// <param name="skip">Page number</param>
    /// <param name="take">Page size</param>
    protected GeneralSpecification(int skip, int take)
    {
    }
}
