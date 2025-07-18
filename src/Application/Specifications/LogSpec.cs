namespace IAVH.BioTablero.CM.Application.Specifications;

using System;

using Ardalis.Specification;

using IAVH.BioTablero.CM.Core.Entities.LogNS;

public class LogSpec : GeneralSpecification<Guid, LogEntity>
{
    public LogSpec()
    {
    }

    public LogSpec(Guid id)
        : base(id)
    {
        Query
            .Where(e => e.Id == id);
    }

    public LogSpec(int skip, int take)
        : base(skip, take)
    {
        Query
            .Skip(skip)
            .Take(take);
    }
}
