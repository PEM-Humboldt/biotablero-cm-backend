namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.InitiativeUser;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

/// <summary>
/// Initiative User edit focus area request example.
/// </summary>
public class InitiativeUserEditFocusAreaRequestExample : IOpenApiExampleProvider<InitiativeUserDto>
{
    /// <inheritdoc/>
    public InitiativeUserDto GetExamples() => new()
    {
        FocusArea = "Focus area example",
    };
}
