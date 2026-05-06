namespace IAVH.BioTablero.CM.Application.DTOs.Resources;

using System;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Resource Like dto.
/// </summary>
public class ResourceLikeDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Resource identifier.
    /// </summary>
    public int ResourceId { get; set; }

    /// <summary>
    /// User Name identifier.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Entity creation date.
    /// </summary>
    public DateTimeOffset? CreationDate { get; set; }
}
