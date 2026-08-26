namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.JoinInvitation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

/// <summary>
/// Join Invitation add request example.
/// </summary>
public class JoinInvitationAddRequestExample : IOpenApiExampleProvider<JoinInvitationDto>
{
    /// <inheritdoc/>
    public JoinInvitationDto GetExamples() => new()
    {
        InitiativeId = 1,
        Message = "Message example (optional)",
        Guests = [
            new()
            {
                Email = "example@example.com",
            }
        ],
    };
}
