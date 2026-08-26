namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.JoinRequest;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Join Request add request example.
/// </summary>
public class JoinRequestAddRequestExample : IOpenApiExampleProvider<JoinRequestDto>
{
    /// <inheritdoc/>
    public JoinRequestDto GetExamples() => new()
    {
        InitiativeId = 1,
        Level = new EnumEntityDto<InitiativeUserLevel>(InitiativeUserLevel.Collaborator),
    };
}
