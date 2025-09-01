namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Initiative response example.
/// </summary>
public class InitiativeResponseExample : IExamplesProvider<InitiativeDto>
{
    /// <summary>
    /// Get examples for entity.
    /// </summary>
    /// <returns>Entity examples.</returns>
    public InitiativeDto GetExamples() => new()
    {
        Id = 0,
        Name = "Initiative example",
        Description = "example",
        CreationDate = DateTime.Now,
        Enabled = true,
        InitiativeLocations = null,
        InitiativeContacts = null,
        InitiativeUsers = null,
    };
}
