namespace IAVH.BioTablero.CM.Core.Domain.Models.Email;

using System.Diagnostics.CodeAnalysis;

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
    [SetsRequiredMembers]
    public CustomEmailAddress(string? name, string email)
    {
        Name = name;
        Email = email;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="email">Receiver email.</param>
    [SetsRequiredMembers]
    public CustomEmailAddress(string email)
    {
        Email = email;
    }

    /// <summary>
    /// Receiver name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Receiver email.
    /// </summary>
    public required string Email { get; set; }
}
