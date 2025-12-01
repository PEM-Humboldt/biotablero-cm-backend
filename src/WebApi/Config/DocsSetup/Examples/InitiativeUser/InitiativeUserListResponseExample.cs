namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.InitiativeUser;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;

using Swashbuckle.AspNetCore.Filters;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Initiative User list response example.
/// </summary>
public class InitiativeUserListResponseExample : IExamplesProvider<List<InitiativeUserDto>>
{
    /// <summary>
    /// Get examples for entity.
    /// </summary>
    /// <returns>Entity examples.</returns>
    public List<InitiativeUserDto> GetExamples() =>
    [
        new()
        {
            Id = 0,
            UserName = "Example",
            FocusArea = "Focus area example",
            Level = new EnumEntityDto<InitiativeUserLevel>(InitiativeUserLevel.Leader),
        }
    ];
}
