namespace IAVH.BioTablero.CM.Application.DTOs.Logging;

using System;

using IAVH.BioTablero.CM.Application.Interfaces.General;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Log base DTO.
/// </summary>
public class LogBaseDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Log creation date.
    /// </summary>
    public DateTimeOffset TimeStamp { get; set; }

    /// <summary>
    /// Custom log type.
    /// </summary>
    public LogType Type { get; set; }

    /// <summary>
    /// Log short message.
    /// </summary>
    public string ShortMessage { get; set; }

    /// <summary>
    /// Event user name.
    /// </summary>
    public string UserName { get; set; }
}
