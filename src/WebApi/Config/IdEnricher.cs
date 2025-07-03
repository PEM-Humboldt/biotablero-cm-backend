namespace IAVH.BioTablero.CM.WebApi.Config;

using System;

using Serilog.Core;
using Serilog.Events;

/// <summary>
/// Serilog custom unique identifier enricher
/// </summary>
public class UniqueIdEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var id = Guid.NewGuid();
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Id", id.ToString()));
    }
}
