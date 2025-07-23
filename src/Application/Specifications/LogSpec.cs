namespace IAVH.BioTablero.CM.Application.Specifications;

using System;

using Ardalis.Specification;

using IAVH.BioTablero.CM.Core.Domain.Entities.LogNS;

/// <summary>
/// Log ardalis specifications
/// </summary>
public class LogSpec : GeneralSpecification<Guid, LogEntity>
{
    /// <summary>
    /// Specification for get element by identifier
    /// </summary>
    /// <param name="id">Element identifier</param>
    public LogSpec(Guid id)
        : base(id)
    {
        Query
            .Where(e => e.Id == id);
    }

    /// <summary>
    /// Specification for get paginated elements
    /// </summary>
    /// <param name="skip">Page number</param>
    /// <param name="take">Page size</param>
    public LogSpec(int skip, int take)
        : base(skip, take)
    {
        Query
            .Skip(skip)
            .Take(take);
    }
}
