namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Initiative OData response example.
/// </summary>
public class InitiativeOdataResponseExample : IExamplesProvider<Dictionary<string, object>>
{
    /// <summary>
    /// Get examples for entity.
    /// </summary>
    /// <returns>Entity examples.</returns>
    public Dictionary<string, object> GetExamples() => new()
    {
        ["@odata.count"] = 1,
        ["value"] = new List<InitiativeDto>()
        {
            new()
            {
                Id = 0,
                Name = "Initiative example",
                Description = "example",
                CreationDate = DateTime.Now,
                Enabled = true,
                InitiativeLocations = null,
                InitiativeContacts = null,
                InitiativeUsers = null,
            },
        },
    };
}
