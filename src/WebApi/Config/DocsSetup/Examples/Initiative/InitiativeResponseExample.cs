namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;

using Swashbuckle.AspNetCore.Filters;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

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
        Contacts = [
            new()
            {
                Id = 0,
                InitiativeId = 0,
                Phone = "3055555555",
                Email = "example@example.com",
            }
        ],
        Users = [
            new()
            {
                Id = 1,
                InitiativeId = 1,
                UserName = "general-admin",
                Level = new EnumEntityDto<InitiativeUserLevel>(InitiativeUserLevel.Leader),
            },
        ],
    };
}
