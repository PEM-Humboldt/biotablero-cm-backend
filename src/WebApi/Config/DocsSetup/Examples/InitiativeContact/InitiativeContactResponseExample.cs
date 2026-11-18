namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.InitiativeContact;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using IAVH.BioTablero.CM.WebApi.Interfaces;

/// <summary>
/// Initiative Contact response example.
/// </summary>
public class InitiativeContactResponseExample : IOpenApiExampleProvider<InitiativeContactDto>
{
    /// <inheritdoc/>
    public InitiativeContactDto GetExamples() => new()
    {
        Id = 0,
        InitiativeId = 0,
        Phone = "3055555555",
        Email = "example@example.com",
    };
}
