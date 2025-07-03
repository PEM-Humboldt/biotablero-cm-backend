namespace IAVH.BioTablero.CM.Core.Entities.LogNS;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.interfaces;

using static IAVH.BioTablero.CM.Core.Enums.LogEnums;

public class LogEntity : BaseEntity<string>, IAggregateRoot
{
    public LogLevel Level { get; set; }
    public DateTimeOffset UtcTimeStamp { get; set; }
    public string RenderedMessage { get; set; }
    public Dictionary<string, object> Properties { get; set; }
}