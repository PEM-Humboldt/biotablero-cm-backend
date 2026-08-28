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
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Logger configuration.
/// </summary>
public static class ConfigLogProperties
{
    private const string SourceContextName = "SourceContext";

    private static readonly string[] ExcludedSourceContexts =
        [

            // EF Core SQL command logs
            "Microsoft.EntityFrameworkCore.Database.Command",

            // HTTP logs
            "Serilog.AspNetCore.RequestLoggingMiddleware",
            "System.Net.Http.HttpClient.ICustomApiKeycloakTokenProvider.ClientHandler",
            "System.Net.Http.HttpClient.ICustomApiKeycloakTokenProvider.LogicalHandler",
            "System.Net.Http.HttpClient.IIamCustomApiService.ClientHandler",
            "System.Net.Http.HttpClient.IIamCustomApiService.LogicalHandler",
            "Microsoft.AspNetCore.Cors.Infrastructure.CorsService",
            "Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker",
            "Microsoft.AspNetCore.Mvc.Infrastructure.ObjectResultExecutor",
            "Microsoft.AspNetCore.Routing.EndpointMiddleware",

            // Hosting logs.
            "Microsoft.AspNetCore.Hosting.Diagnostics",
            "Microsoft.Hosting.Lifetime",

            // Key management logs
            "Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager",

            // Auth logs
            "Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerHandler",
            "Microsoft.AspNetCore.Authorization.DefaultAuthorizationService",
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
                        .Filter.ByExcluding(e =>
                            e.Properties.TryGetValue(SourceContextName, out var sourceContext) &&
                            sourceContext is ScalarValue { Value: string source } &&
                            ExcludedSourceContexts.Contains(source))

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
