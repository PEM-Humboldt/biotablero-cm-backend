namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.JoinInvitation;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

/// <summary>
/// Join Invitation response example.
/// </summary>
public class JoinInvitationResponseExample : IOpenApiExampleProvider<JoinInvitationDto>
{
    /// <inheritdoc/>
    public JoinInvitationDto GetExamples() => new()
    {
        Id = 0,
        InitiativeId = 0,
        Creator = "Example",
        Message = "Message example",
        CreationDate = DateTimeOffset.UtcNow,
        Guests = [
            new()
            {
                Id = 0,
                JoinInvitationId = 0,
                Email = "example@example.com",
            }
        ],
    };
}
