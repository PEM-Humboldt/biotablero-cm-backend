namespace IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Initiative Contact dto.
/// </summary>
public class InitiativeContactDto : IDto
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
    public string Phone { get; set; }

    /// <summary>
    /// Email address.
    /// </summary>
    public string Email { get; set; }
}
