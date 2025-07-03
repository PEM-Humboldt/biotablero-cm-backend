namespace IAVH.BioTablero.CM.Application.Specifications;

using Ardalis.Specification;

using IAVH.BioTablero.CM.Core.Entities.LogNS;


public class LogSpec : GeneralSpecification<string, LogEntity>
{
    public LogSpec() { }

    public LogSpec(string id) : base(id)
    {
        Query
            .Where(e => e.Id == id);
    }

    public LogSpec(int skip, int take) : base(skip, take)
    {
        Query
            .Skip(skip)
            .Take(take);
    }
}