namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.InitiativeUser;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;

using Swashbuckle.AspNetCore.Filters;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Initiative User edit request example.
/// </summary>
public class InitiativeUserEditRequestExample : IExamplesProvider<InitiativeUserDto>
{
    /// <inheritdoc/>
    public InitiativeUserDto GetExamples() => new()
    {
        FocusArea = "Focus area example",
        Level = new EnumEntityDto<InitiativeUserLevel>(InitiativeUserLevel.Leader),
    };
}
