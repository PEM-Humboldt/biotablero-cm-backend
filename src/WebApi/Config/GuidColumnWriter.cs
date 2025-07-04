namespace IAVH.BioTablero.CM.WebApi.Config;

using System;

using NpgsqlTypes;

using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

public class GuidColumnWriter : ColumnWriterBase
{
    private readonly string propertyName;

    public GuidColumnWriter(string propertyName, NpgsqlDbType dbType = NpgsqlDbType.Uuid)
        : base(dbType)
    {
        this.propertyName = propertyName;
    }

    public override object GetValue(LogEvent logEvent, IFormatProvider formatProvider = null)
    {
        if (logEvent.Properties.TryGetValue(propertyName, out var value))
        {
            if (value is ScalarValue scalar && scalar.Value is Guid guidValue)
            {
                return guidValue;
            }
        }

        throw new InvalidCastException("Property is missing or not a Guid.");
    }
}