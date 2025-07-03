namespace IAVH.BioTablero.CM.Core.DTOs.LogNS;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.interfaces;

using static IAVH.BioTablero.CM.Core.Enums.LogEnums;

public class LogDto : IDto
{
    public string Id { get; set; }
    public LogLevel Level { get; set; }
    public DateTimeOffset UtcTimeStamp { get; set; }
    public string RenderedMessage { get; set; }
    public Dictionary<string, object> Properties { get; set; }
}