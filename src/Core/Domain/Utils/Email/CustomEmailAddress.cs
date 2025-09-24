namespace IAVH.BioTablero.CM.Core.Domain.Utils.Email;

/// <summary>
/// Custom email address data.
/// </summary>
/// <param name="name">Receiver name.</param>
/// <param name="email">Receiver email.</param>
public class CustomEmailAddress(string name, string email)
{
    /// <summary>
    /// Receiver name.
    /// </summary>
    public string Name { get; set; } = name;

    /// <summary>
    /// Receiver email.
    /// </summary>
    public string Email { get; set; } = email;
}
