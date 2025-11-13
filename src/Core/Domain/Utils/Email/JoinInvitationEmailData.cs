namespace IAVH.BioTablero.CM.Core.Domain.Utils.Email;

/// <summary>
/// Join invitation email data.
/// </summary>
public class JoinInvitationEmailData : DefaultEmailData
{
    /// <summary>
    /// Initiative name.
    /// </summary>
    public string InitiativeName { get; set; }

    /// <summary>
    /// Email message.
    /// </summary>
    public string EmailMessage { get; set; }
}
