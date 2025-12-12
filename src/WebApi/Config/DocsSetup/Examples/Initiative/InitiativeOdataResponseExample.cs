namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

/// <summary>
/// Initiative OData response example.
/// </summary>
public class InitiativeOdataResponseExample : BaseOdataResponseExample<InitiativeDto>
{
    /// <summary>
    /// Create example DTO object.
    /// </summary>
    /// <returns>Example DTO object.</returns>
    protected override InitiativeDto CreateExampleDto() => new()
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
    };
}
