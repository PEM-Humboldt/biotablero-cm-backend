namespace IAVH.BioTablero.CM.Application.DTOs.Users;

using System;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// External User DTO Base (IAM).
/// </summary>
public class ExternalUserBaseDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// User email.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// User name.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// User full name.
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// User picture.
    /// </summary>
    public string Picture { get; set; }
}
