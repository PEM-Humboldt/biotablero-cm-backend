namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Initiative list response example.
/// </summary>
public class InitiativeListResponseExample : IExamplesProvider<List<InitiativeDto>>
{
    /// <summary>
    /// Get examples for entity.
    /// </summary>
    /// <returns>Entity examples.</returns>
    public List<InitiativeDto> GetExamples() =>
    [
        new()
        {
            Id = 0,
            Name = "Initiative example",
            ShortName = "IE",
            Description = "example",
            InfluenceArea = "Influence area example",
            Objective = "Objective example",
            CreationDate = DateTime.Now,
            Enabled = true,
            Locations = null,
            Contacts = null,
            Users = null,
        }
    ];
}
