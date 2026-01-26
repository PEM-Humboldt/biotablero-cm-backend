namespace IAVH.BioTablero.CM.Core.Domain.Models.Email;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

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
    /// Join request status.
    /// </summary>
    public JoinRequestStatus JoinRequestStatus { get; set; }
}
