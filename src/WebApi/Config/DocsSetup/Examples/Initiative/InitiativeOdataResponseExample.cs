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
        Description = "example",
        CreationDate = DateTime.Now,
        Coordinate = [-75.3, 5.3],
        PolygonArea = 62.7,
        Enabled = true,
        Locations = [
            new()
            {
                Id = 0,
                LocationId = 0,
                Locality = "Locality example",
                Location = new()
                {
                    Id = 0,
                    Name = "Example",
                    Code = "000",
                    Parent = new()
                    {
                        Id = 0,
                        Name = "Example",
                        Code = "000",
                    },
                },
            },
        ],
        Contacts = null,
        Users = null,
    };
}
