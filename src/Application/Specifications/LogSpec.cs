namespace IAVH.BioTablero.CM.Application.Specifications;

using System;

using IAVH.BioTablero.CM.Core.Domain.Entities.Logging;

/// <summary>
/// Log ardalis specifications.
/// </summary>
public class LogSpec : GeneralSpecification<Guid, LogEntity>
{
    /// <summary>
    /// Specification for get element by identifier.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    public LogSpec(Guid id)
        : base(id)
    {
    }
}
