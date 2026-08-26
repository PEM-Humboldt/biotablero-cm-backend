namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.InitiativeContact;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

/// <summary>
/// Initiative Contact add request example.
/// </summary>
public class InitiativeContactAddRequestExample : IOpenApiExampleProvider<InitiativeContactDto>
{
    /// <inheritdoc/>
    public InitiativeContactDto GetExamples() => new()
    {
        InitiativeId = 1,
        Phone = "3055555555",
        Email = "example@example.com",
    };
}
