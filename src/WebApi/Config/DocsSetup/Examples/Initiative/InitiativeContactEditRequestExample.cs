namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Initiative Contact edit request example.
/// </summary>
public class InitiativeContactEditRequestExample : IExamplesProvider<InitiativeContactDto>
{
    /// <summary>
    /// Get examples for entity.
    /// </summary>
    /// <returns>Entity examples.</returns>
    public InitiativeContactDto GetExamples() => new()
    {
        Phone = "3055555555",
        Email = "example@example.com",
    };
}
