namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Auth;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;
using IAVH.BioTablero.CM.WebApi.Interfaces;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Initiative logged user data response example.
/// </summary>
public class InitiativeLoggedUserDataResponseExample : IOpenApiExampleProvider<List<InitiativeDto>>
{
    /// <inheritdoc/>
    public List<InitiativeDto> GetExamples() =>
    [
        new()
        {
            Id = 0,
            Name = "Initiative example",
            Description = "example",
            CreationDate = DateTimeOffset.UtcNow,
            Enabled = true,
            Locations = null,
            Contacts = null,
            Users = [
                new()
                {
                    Id = 0,
                    UserName = "initiative-leader@example.com",
                    Level = new EnumEntityDto<InitiativeUserLevel>(InitiativeUserLevel.Leader),
                }
            ],
        }
    ];
}
