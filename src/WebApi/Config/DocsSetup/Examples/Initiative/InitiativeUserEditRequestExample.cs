namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;

using Swashbuckle.AspNetCore.Filters;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Initiative User edit request example.
/// </summary>
public class InitiativeUserEditRequestExample : IExamplesProvider<InitiativeUserDto>
{
    /// <summary>
    /// Get examples for entity.
    /// </summary>
    /// <returns>Entity examples.</returns>
    public InitiativeUserDto GetExamples() => new()
    {
        UserName = "Example",
        Level = new EnumEntityDto<InitiativeUserLevel>(InitiativeUserLevel.Leader),
    };
}
