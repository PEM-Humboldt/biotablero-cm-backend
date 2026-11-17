namespace IAVH.BioTablero.CM.WebApi.Config.LoggerSetup.ColumWriters;

using System;

using NpgsqlTypes;

using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

/// <summary>
/// Custom Serilog column writer for Timestamp.
/// </summary>
public sealed class UtcTimestampColumnWriter : ColumnWriterBase
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public UtcTimestampColumnWriter()
        : base(NpgsqlDbType.TimestampTz)
    {
    }

    /// <inheritdoc/>
    public override object GetValue(
        LogEvent logEvent,
        IFormatProvider? formatProvider = null) =>
            logEvent.Timestamp.UtcDateTime;
}
