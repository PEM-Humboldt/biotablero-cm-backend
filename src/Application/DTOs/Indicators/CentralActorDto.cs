namespace IAVH.BioTablero.CM.Application.DTOs.Indicators;

using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Central Actor dto.
/// </summary>
[method: SetsRequiredMembers]
public class CentralActorDto() : IDto
{
    /// <summary>
    /// Central Actor name.
    /// </summary>
    public required string Name { get; set; } = string.Empty;
}
