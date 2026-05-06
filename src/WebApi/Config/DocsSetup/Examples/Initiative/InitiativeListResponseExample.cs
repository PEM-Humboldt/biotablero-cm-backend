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
    /// <inheritdoc/>
    public List<InitiativeDto> GetExamples() =>
    [
        new()
        {
            Id = 0,
            Name = "Initiative example",
            ShortName = "IE",
            Description = "example",
            Baseline = "Baseline example",
            Objective = "Objective example",
            CreationDate = DateTimeOffset.UtcNow,
            Enabled = true,
            Locations = null,
            Contacts = null,
            Users = null,
        }
    ];
}
