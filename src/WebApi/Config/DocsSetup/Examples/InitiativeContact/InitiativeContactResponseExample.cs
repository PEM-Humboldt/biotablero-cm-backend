namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.InitiativeContact;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Initiative Contact response example.
/// </summary>
public class InitiativeContactResponseExample : IExamplesProvider<InitiativeContactDto>
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
