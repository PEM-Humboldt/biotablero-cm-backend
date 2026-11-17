namespace IAVH.BioTablero.CM.Application.DTOs.Reports;

using System;
using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Report Data dto.
/// </summary>
[method: SetsRequiredMembers]
public class ReportDataDto() : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Report creation date.
    /// </summary>
    public DateTimeOffset? CreationDate { get; set; }

    /// <summary>
    /// Creator user name.
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// Report generation description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Report generation data.
    /// </summary>
    public string? Data { get; set; }
}
