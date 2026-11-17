namespace IAVH.BioTablero.CM.Application.DTOs.Reports;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.DTOs.Tags;

/// <summary>
/// Ecosystems statistics DTO.
/// </summary>
[method: SetsRequiredMembers]
public class EcosystemsStatsDto()
{
    /// <summary>
    /// Ecosystems involved.
    /// </summary>
    public List<TagDto> EcosystemsInvolved { get; set; } = [];
}
