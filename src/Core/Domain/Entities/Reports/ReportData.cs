namespace IAVH.BioTablero.CM.Core.Domain.Entities.Reports;

using System;

using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Report Data entity.
/// </summary>
public class ReportData : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Report creation date.
    /// </summary>
    public DateTimeOffset CreationDate { get; set; }

    /// <summary>
    /// Creator user name.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Report generation description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Report generation data.
    /// </summary>
    public string Data { get; set; }
}
