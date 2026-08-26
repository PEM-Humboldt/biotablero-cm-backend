namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.InitiativeUser;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;

using IAVH.BioTablero.CM.WebApi.Interfaces;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Initiative User add request example.
/// </summary>
public class InitiativeUserAddRequestExample : IOpenApiExampleProvider<InitiativeUserDto>
{
    /// <inheritdoc/>
    public InitiativeUserDto GetExamples() => new()
    {
        InitiativeId = 1,
        UserName = "initiative-collaborator@example.com",
        FocusArea = "Focus area example",
        Level = new EnumEntityDto<InitiativeUserLevel>(InitiativeUserLevel.Collaborator),
    };
}
