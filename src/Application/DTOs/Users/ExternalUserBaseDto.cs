namespace IAVH.BioTablero.CM.Application.DTOs.Users;

using System;
using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// External User DTO Base (IAM).
/// </summary>
[method: SetsRequiredMembers]
public class ExternalUserBaseDto() : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// User email.
    /// </summary>
    public required string Email { get; set; } = string.Empty;

    /// <summary>
    /// User name.
    /// </summary>
    public required string Username { get; set; } = string.Empty;

    /// <summary>
    /// User full name.
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// User picture.
    /// </summary>
    public string? Picture { get; set; }
}
