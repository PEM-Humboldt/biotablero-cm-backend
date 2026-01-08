namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.InitiativeLocation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Initiative Location response example.
/// </summary>
public class InitiativeLocationResponseExample : IExamplesProvider<InitiativeLocationDto>
{
    /// <inheritdoc/>
    public InitiativeLocationDto GetExamples() => new()
    {
        Id = 0,
        InitiativeId = 0,
        LocationId = 0,
        Locality = "Locality example",
        Location = new()
        {
            Id = 0,
            Name = "Example",
            Code = "000",
            Parent = new()
            {
                Id = 0,
                Name = "Example",
                Code = "000",
            },
        },
    };
}
