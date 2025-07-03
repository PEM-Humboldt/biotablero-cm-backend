namespace IAVH.BioTablero.CM.Core.Entities.LogNS;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.interfaces;

using static IAVH.BioTablero.CM.Core.Enums.LogEnums;

public class LogEntity : BaseEntity<Guid>, IAggregateRoot
{
    public LogLevel Level { get; set; }
    public DateTimeOffset TimeStamp { get; set; }
    public string Message { get; set; }
    public Dictionary<string, object> Properties { get; set; }
}