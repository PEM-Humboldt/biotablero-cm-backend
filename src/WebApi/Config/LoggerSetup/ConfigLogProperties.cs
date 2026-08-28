namespace IAVH.BioTablero.CM.WebApi.Config.LoggerSetup;

using System;
using System.Collections.Generic;
using System.Globalization;

using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.WebApi.Config.LoggerSetup.ColumWriters;
using IAVH.BioTablero.CM.WebApi.Config.LoggerSetup.Enrichers;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using NpgsqlTypes;

using Serilog;
using Serilog.Enrichers;
using Serilog.Sinks.PostgreSQL;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Logger configuration.
/// </summary>
public static class ConfigLogProperties
{
    private const string SourceContextName = "SourceContext";

    /// <summary>
    /// System log configuration.
    /// </summary>
    /// <param name="host">Host builder.</param>
    /// <returns>Host builder configuration.</returns>
    public static ConfigureHostBuilder AddLogConfig(this ConfigureHostBuilder host)
    {
        var columnWriters = new Dictionary<string, ColumnWriterBase>
        {
            { "id", new GuidColumnWriter("Id") },
            { "timestamp", new UtcTimestampColumnWriter() },
            { "level", new LevelColumnWriter() },
            { "type", new IntegerColumnWriter(LogConstants.CustomType) },
            { "message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
            { "short_message", new RawStringColumnWriter(LogConstants.ShortMessage) },
            { "user_name", new RawStringColumnWriter("UserName") },
            { "custom_record", new BoolColumnWriter(LogConstants.CustomRecord) },
            { "client_ip", new RawStringColumnWriter("ClientIp") },
            { "client_agent", new RawStringColumnWriter("ClientAgent") },
            { "properties", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) },
        };

        // General setup
        host.UseSerilog((context, serviceProvider, loggerConfiguration) =>
            {
                loggerConfiguration
                    .Enrich.With<IdEnricher>()
                    .Enrich.WithProperty(LogConstants.ApplicationName, LogConstants.ProjectName)
                    .Enrich.WithProperty(LogConstants.CustomRecord, false)
                    .Enrich.WithProperty(LogConstants.CustomType, (int)LogType.System)
                    .Enrich.With(new UserEnricher(serviceProvider.GetRequiredService<IHttpContextAccessor>()))
                    .Enrich.WithMachineName()
                    .Enrich.With(new ClientIpEnricher())
                    .Enrich.With(new ClientHeaderEnricher("User-Agent", "ClientAgent"))
                    .ReadFrom.Configuration(context.Configuration)

                    .WriteTo.Logger(lc => lc

                        // Discard EF Core SQL command logs
                        .Filter.ByExcluding(e =>
                            e.Properties.TryGetValue(SourceContextName, out var sourceContext) &&
                            sourceContext.ToString().Contains(
                                "Microsoft.EntityFrameworkCore.Database.Command",
                                StringComparison.OrdinalIgnoreCase))

                        // Discard Serilog HTTP request logs
                        .Filter.ByExcluding(e =>
                            e.Properties.TryGetValue(SourceContextName, out var sourceContext) &&
                            sourceContext.ToString().Contains(
                                "Serilog.AspNetCore.RequestLoggingMiddleware",
                                StringComparison.OrdinalIgnoreCase))

                        .WriteTo.PostgreSQL(
                            connectionString: EnvUtils.GetRequiredEnv("CS_MAIN"),
                            schemaName: LogConstants.DefaultSchemaName,
                            tableName: LogConstants.DefaultTableName,
                            needAutoCreateTable: false,
                            columnOptions: columnWriters,
                            formatProvider: CultureInfo.CurrentCulture))

                    .WriteTo.Console(
                        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] [{Id}] {Message:lj}{NewLine}",
                        formatProvider: CultureInfo.CurrentCulture);
            });

        return host;
    }
}
