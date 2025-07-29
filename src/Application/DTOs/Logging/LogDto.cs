namespace IAVH.BioTablero.CM.Application.DTOs.Logging;

using System;

using IAVH.BioTablero.CM.Application.Interfaces.General;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Serilog log DTO
/// </summary>
public class LogDto : IDto
{
    /// <summary>
    /// Item identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Log creation date
    /// </summary>
    public DateTime TimeStamp { get; set; }

    /// <summary>
    /// Serilog level
    /// </summary>
    public LogLevel Level { get; set; }

    /// <summary>
    /// Custom log type
    /// </summary>
    public LogType? Type { get; set; }

    /// <summary>
    /// Log message
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Event user name
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Indicates whether the record was created manually or not
    /// </summary>
    public bool CustomRecord { get; set; }

    /// <summary>
    /// Event client IP
    /// </summary>
    public string ClientIp { get; set; }

    /// <summary>
    /// Event client agent (web browser)
    /// </summary>
    public string ClientAgent { get; set; }

    /// <summary>
    /// Log details
    /// </summary>
    public string Properties { get; set; }
}
