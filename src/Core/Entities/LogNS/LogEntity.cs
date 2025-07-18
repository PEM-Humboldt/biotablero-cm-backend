namespace IAVH.BioTablero.CM.Core.Entities.LogNS;

using System;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

using static IAVH.BioTablero.CM.Core.Enums.LogEnums;

public class LogEntity : BaseEntity<Guid>, IAggregateRoot
{
    public DateTime TimeStamp { get; set; }
    public LogLevel Level { get; set; }
    public LogType? Type { get; set; }
    public string Message { get; set; }
    public string UserName { get; set; }
    public bool CustomRecord { get; set; }
    public string ClientIp { get; set; }
    public string ClientAgent { get; set; }
    public string Properties { get; set; }
}
