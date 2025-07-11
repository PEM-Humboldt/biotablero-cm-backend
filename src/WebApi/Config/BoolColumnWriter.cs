namespace IAVH.BioTablero.CM.WebApi.Config;

using System;

using NpgsqlTypes;

using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

/// <summary>
/// Custom Serilog column writer for Boolean data
/// </summary>
/// <param name="propertyName">Serilog property name</param>
/// <param name="dbType">Column database type</param>
public class BoolColumnWriter(string propertyName, NpgsqlDbType dbType = NpgsqlDbType.Boolean) : ColumnWriterBase(dbType)
{
    private readonly string propertyName = propertyName;

    /// <summary>
    /// Get data value
    /// </summary>
    /// <param name="logEvent">Log event data</param>
    /// <param name="formatProvider">Format provider</param>
    /// <returns>Scalar property value</returns>
    public override object GetValue(LogEvent logEvent, IFormatProvider formatProvider = null)
    {
        if (logEvent.Properties.TryGetValue(propertyName, out var value))
        {
            if (value is ScalarValue scalar && scalar.Value is bool guidValue)
            {
                return guidValue;
            }
        }

        throw new InvalidCastException("Property is missing or not a Boolean.");
    }
}
