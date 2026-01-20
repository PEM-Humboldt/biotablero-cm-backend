namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.JoinInvitation;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

/// <summary>
/// Join Invitation OData response example.
/// </summary>
public class JoinInvitationOdataResponseExample : BaseOdataResponseExample<JoinInvitationDto>
{
    /// <inheritdoc/>
    protected override JoinInvitationDto CreateExampleDto() => new()
    {
        Id = 0,
        InitiativeId = 0,
        Creator = "Example",
        Message = "Message example",
        CreationDate = DateTime.Now,
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
