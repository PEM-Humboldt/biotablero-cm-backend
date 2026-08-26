namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.InitiativeUser;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Initiative User response example.
/// </summary>
public class InitiativeUserResponseExample : IOpenApiExampleProvider<InitiativeUserDto>
{
    /// <inheritdoc/>
    public InitiativeUserDto GetExamples() => new()
    {
        Id = 0,
        UserName = "initiative-leader@example.com",
        FocusArea = "Focus area example",
        Level = new EnumEntityDto<InitiativeUserLevel>(InitiativeUserLevel.Leader),
    };
}
