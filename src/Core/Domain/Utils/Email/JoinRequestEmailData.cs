namespace IAVH.BioTablero.CM.Core.Domain.Utils.Email;

/// <summary>
/// Join request email data.
/// </summary>
public class JoinRequestEmailData : DefaultEmailData
{
    /// <summary>
    /// Initiative name.
    /// </summary>
    public string InitiativeName { get; set; }

    /// <summary>
    /// User name.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Join request status: "Created", "Approved", "Rejected".
    /// </summary>
    public string JoinRequestStatus { get; set; }
}
