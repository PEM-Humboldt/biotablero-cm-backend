namespace IAVH.BioTablero.CM.Core.DTOs.LogNS;

using System;

using IAVH.BioTablero.CM.Core.interfaces;

using static IAVH.BioTablero.CM.Core.Enums.LogEnums;

public class LogDto : IDto
{
    public Guid Id { get; set; }
    public DateTime TimeStamp { get; set; }
    public LogLevel Level { get; set; }
    public string Message { get; set; }
    public string UserName { get; set; }
    public bool CustomRecord { get; set; }
    public string ClientIp { get; set; }
    public string ClientAgent { get; set; }
    public string Properties { get; set; }
}