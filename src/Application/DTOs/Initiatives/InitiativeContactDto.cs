namespace IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Initiative Contact dto.
/// </summary>
[method: SetsRequiredMembers]
public class InitiativeContactDto() : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public int? InitiativeId { get; set; }

    /// <summary>
    /// Phone number.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Email address.
    /// </summary>
    public required string Email { get; set; } = string.Empty;
}
