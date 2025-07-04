namespace IAVH.BioTablero.CM.WebApi.Config;

using System;

using NpgsqlTypes;

using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

public class RawStringColumnWriter : ColumnWriterBase
{
    private readonly string propertyName;

    public RawStringColumnWriter(string propertyName, NpgsqlDbType dbType = NpgsqlDbType.Text)
        : base(dbType)
    {
        this.propertyName = propertyName;
    }

    public override object GetValue(LogEvent logEvent, IFormatProvider formatProvider = null)
    {
        if (logEvent.Properties.TryGetValue(propertyName, out var value))
        {
            if (value is ScalarValue scalar)
            {
                // Devolver la cadena sin comillas
                return scalar.Value?.ToString();
            }
            return value.ToString();
        }

        return null; // O devuelve "anonymous" si prefieres
    }
}