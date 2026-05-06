namespace IAVH.BioTablero.CM.Core.Domain.Models.Iam;

using System;

/// <summary>
/// Extarnal User data (IAM).
/// </summary>
public class ExternalUser
{
    /// <summary>
    /// User identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// User email.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Email verified flag.
    /// </summary>
    public bool EmailVerified { get; set; }

    /// <summary>
    /// User name.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// User first name.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// User last name.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// User full name.
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>
    /// User creation date.
    /// </summary>
    public DateTimeOffset? CreationDate { get; set; }
}
