namespace IAVH.BioTablero.CM.WebApi.Config.LoggerSetup.ColumWriters;

using System;

using NpgsqlTypes;

using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

/// <summary>
/// Custom Serilog column writer for strings.
/// </summary>
/// <param name="propertyName">Serilog property name.</param>
/// <param name="dbType">Column database type.</param>
public class RawStringColumnWriter(string propertyName, NpgsqlDbType dbType = NpgsqlDbType.Text) : ColumnWriterBase(dbType)
{
    private readonly string propertyName = propertyName;

    /// <inheritdoc/>
    public override object GetValue(LogEvent logEvent, IFormatProvider formatProvider = null)
    {
        if (logEvent != null && logEvent.Properties.TryGetValue(propertyName, out var value))
        {
            if (value is ScalarValue scalar)
            {
                return scalar.Value?.ToString();
            }

            return value.ToString();
        }

        return null;
    }
}
