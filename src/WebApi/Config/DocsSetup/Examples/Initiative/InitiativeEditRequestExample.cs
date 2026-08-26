namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

/// <summary>
/// Initiative edit response example.
/// </summary>
public class InitiativeEditRequestExample : IOpenApiExampleProvider<InitiativeDto>
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
