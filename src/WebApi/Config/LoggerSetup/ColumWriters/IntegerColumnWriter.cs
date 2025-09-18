namespace IAVH.BioTablero.CM.WebApi.Config.LoggerSetup.ColumWriters;

using System;

using NpgsqlTypes;

using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

/// <summary>
/// Custom Serilog column writer for Integer data.
/// </summary>
/// <param name="propertyName">Serilog property name.</param>
/// <param name="dbType">Column database type.</param>
public class IntegerColumnWriter(string propertyName, NpgsqlDbType dbType = NpgsqlDbType.Integer) : ColumnWriterBase(dbType)
{
    private readonly string propertyName = propertyName;

    /// <summary>
    /// Get data value.
    /// </summary>
    /// <param name="logEvent">Log event data.</param>
    /// <param name="formatProvider">Format provider.</param>
    /// <returns>Scalar property value.</returns>
    public override object GetValue(LogEvent logEvent, IFormatProvider formatProvider = null)
    {
        if (logEvent != null && logEvent.Properties.TryGetValue(propertyName, out var value) && value is ScalarValue scalar && scalar.Value is int intValue)
        {
            return intValue;
        }

        throw new InvalidCastException("Property is missing or not a Integer.");
    }
}
