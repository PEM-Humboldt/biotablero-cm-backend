namespace IAVH.BioTablero.CM.WebApi.Config.LoggerSetup.Enrichers;

using System;

using Serilog.Core;
using Serilog.Events;

/// <summary>
/// Serilog custom unique identifier enricher.
/// </summary>
public class IdEnricher : ILogEventEnricher
{
    /// <inheritdoc/>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var id = Guid.NewGuid();
        var idProperty = new LogEventProperty("Id", new ScalarValue(id));

        logEvent?.AddOrUpdateProperty(idProperty);
    }
}
