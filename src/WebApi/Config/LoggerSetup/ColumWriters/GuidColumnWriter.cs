namespace IAVH.BioTablero.CM.WebApi.Config.LoggerSetup.ColumWriters;

using System;

using NpgsqlTypes;

using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

/// <summary>
/// Custom Serilog column writer for GUID data.
/// </summary>
/// <param name="propertyName">Serilog property name.</param>
/// <param name="dbType">Column database type.</param>
public class GuidColumnWriter(string propertyName, NpgsqlDbType dbType = NpgsqlDbType.Uuid) : ColumnWriterBase(dbType)
{
    private readonly string propertyName = propertyName;

    /// <inheritdoc/>
    public override object GetValue(LogEvent logEvent, IFormatProvider formatProvider = null)
    {
        if (logEvent != null && logEvent.Properties.TryGetValue(propertyName, out var value) && value is ScalarValue scalar && scalar.Value is Guid guidValue)
        {
            return guidValue;
        }

        throw new InvalidCastException("Property is missing or not a Guid.");
    }
}
