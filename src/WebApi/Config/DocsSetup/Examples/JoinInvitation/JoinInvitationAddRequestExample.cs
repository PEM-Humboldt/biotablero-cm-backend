namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.JoinInvitation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Join Invitation add request example.
/// </summary>
public class JoinInvitationAddRequestExample : IExamplesProvider<JoinInvitationDto>
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
