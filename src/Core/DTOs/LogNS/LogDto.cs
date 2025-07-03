namespace IAVH.BioTablero.CM.Core.DTOs.LogNS;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.interfaces;

using static IAVH.BioTablero.CM.Core.Enums.LogEnums;

public class LogDto : IDto
{
    public Guid Id { get; set; }
    public DateTimeOffset TimeStamp { get; set; }
    public LogLevel Level { get; set; }
    public string Message { get; set; }
    public Dictionary<string, object> Properties { get; set; }
}