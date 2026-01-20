namespace IAVH.BioTablero.CM.Core.Domain.Models.Email;

/// <summary>
/// Custom email address data.
/// </summary>
public class CustomEmailAddress
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">Receiver name.</param>
    /// <param name="email">Receiver email.</param>
    public CustomEmailAddress(string name, string email)
    {
        Name = name;
        Email = email;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="email">Receiver email.</param>
    public CustomEmailAddress(string email)
    {
        Email = email;
    }

    /// <summary>
    /// Receiver name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Receiver email.
    /// </summary>
    public string Email { get; set; }
}
