namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Initiative Contact add request example.
/// </summary>
public class InitiativeContactAddRequestExample : IExamplesProvider<InitiativeContactDto>
{
    /// <summary>
    /// Get examples for entity.
    /// </summary>
    /// <returns>Entity examples.</returns>
    public InitiativeContactDto GetExamples() => new()
    {
        InitiativeId = 1,
        Phone = "3055555555",
        Email = "example@example.com",
    };
}
