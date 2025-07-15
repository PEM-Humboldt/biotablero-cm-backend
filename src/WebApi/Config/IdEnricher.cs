namespace IAVH.BioTablero.CM.WebApi.Config;

using System;

using Serilog.Core;
using Serilog.Events;

/// <summary>
/// Serilog custom unique identifier enricher
/// </summary>
public class IdEnricher : ILogEventEnricher
{
    /// <summary>
    /// Enrich logs with custom identifier
    /// </summary>
    /// <param name="logEvent">Serilog log event</param>
    /// <param name="propertyFactory">Serilog property factory</param>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var id = Guid.NewGuid();
        var idProperty = new LogEventProperty("Id", new ScalarValue(id));

        logEvent.AddOrUpdateProperty(idProperty);
    }
}
