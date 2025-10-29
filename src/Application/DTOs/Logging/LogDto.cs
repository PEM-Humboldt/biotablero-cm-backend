namespace IAVH.BioTablero.CM.Application.DTOs.Logging;

using IAVH.BioTablero.CM.Application.Interfaces.General;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Serilog log DTO.
/// </summary>
public class LogDto : LogBaseDto, IDto
{
    /// <summary>
    /// Serilog level.
    /// </summary>
    public LogLevel Level { get; set; }

    /// <summary>
    /// Indicates whether the record was created manually or not.
    /// </summary>
    public bool CustomRecord { get; set; }

    /// <summary>
    /// Event client IP.
    /// </summary>
    public string ClientIp { get; set; }

    /// <summary>
    /// Event client agent (web browser).
    /// </summary>
    public string ClientAgent { get; set; }

    /// <summary>
    /// Log details.
    /// </summary>
    public string Properties { get; set; }
}
