namespace IAVH.BioTablero.CM.Core.Domain.Models.Email;

/// <summary>
/// User removal email data.
/// </summary>
public class UserRemovalEmailData : DefaultEmailData
{
    /// <summary>
    /// Initiative name.
    /// </summary>
    public string InitiativeName { get; set; }
}
