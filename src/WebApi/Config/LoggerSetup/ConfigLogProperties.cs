namespace IAVH.BioTablero.CM.WebApi.Config.LoggerSetup;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Logger configuration.
/// </summary>
public static class ConfigLogProperties
{
    private static readonly string[] ExcludedSourceContexts =
    [
        "Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerHandler",
        "Microsoft.AspNetCore.Authorization.DefaultAuthorizationService",
        "Microsoft.AspNetCore.Cors.Infrastructure.CorsService",
        "Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager",
        "Microsoft.AspNetCore.Hosting.Diagnostics",
        "Microsoft.AspNetCore.Routing.EndpointMiddleware",
        "Microsoft.EntityFrameworkCore.Database.Command",
        "Microsoft.Hosting.Lifetime",
        "Serilog.AspNetCore.RequestLoggingMiddleware",
    ];

    private static readonly string[] ExcludedSourceContextPrefixes =
    [
        "Microsoft.AspNetCore.Mvc.Infrastructure",
        "System.Net.Http.HttpClient.ICustomApiKeycloakTokenProvider",
        "System.Net.Http.HttpClient.IIamCustomApiService",
        "System.Net.Http.HttpClient.IIamService",
        "System.Net.Http.HttpClient.IKeycloakTokenProvider",
    ];

    private static readonly LogEventLevel[] DbLogLevels =
    [
        LogEventLevel.Warning,
        LogEventLevel.Error,
        LogEventLevel.Fatal,
    ];

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
                        .Filter.ByExcluding(ShouldExclude)

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

    /// <summary>
    /// Check whether a record should be excluded.
    /// </summary>
    /// <param name="logEvent">Serilog log event.</param>
    /// <returns>True if the log should be excluded. False otherwise.</returns>
    private static bool ShouldExclude(LogEvent logEvent)
    {
        if (DbLogLevels.Contains(logEvent.Level))
        {
            return false;
        }

        if (!logEvent.Properties.TryGetValue(LogConstants.SourceContext, out var sourceContext) ||
            sourceContext is not ScalarValue { Value: string source })
        {
            return false;
        }

        if (logEvent.Properties[LogConstants.CustomRecord] is ScalarValue { Value: bool customRecordValue } &&
            customRecordValue)
        {
            return false;
        }

        return ExcludedSourceContexts.Contains(source) ||
            ExcludedSourceContextPrefixes.Any(prefix =>
                source.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
    }
}
