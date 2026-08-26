namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

/// <summary>
/// Initiative list response example.
/// </summary>
public class InitiativeListResponseExample : IOpenApiExampleProvider<List<InitiativeDto>>
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
            HasPolygon = false,
            Locations = null,
            Contacts = null,
            Users = null,
        }
    ];
}
