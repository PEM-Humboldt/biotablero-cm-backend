namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Initiative edit response example.
/// </summary>
public class InitiativeEditRequestExample : IExamplesProvider<InitiativeDto>
{
    /// <inheritdoc/>
    public InitiativeDto GetExamples() => new()
    {
        Name = "Initiative example (edited)",
        ShortName = "IE",
        Description = "example",
        Baseline = "Baseline example",
        Objective = "Objective example",
        Locations = null,
        Contacts = null,
        Users = null,
    };
}
