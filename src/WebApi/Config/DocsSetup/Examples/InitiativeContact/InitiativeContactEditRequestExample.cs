namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.InitiativeContact;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Initiative Contact edit request example.
/// </summary>
public class InitiativeContactEditRequestExample : IExamplesProvider<InitiativeContactDto>
{
    /// <inheritdoc/>
    public InitiativeContactDto GetExamples() => new()
    {
        Phone = "3055555555",
        Email = "example@example.com",
    };
}
